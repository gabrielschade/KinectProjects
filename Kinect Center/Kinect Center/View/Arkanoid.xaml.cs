using Kinect_Center.Business.Classes;
using Kinect_Center.Business.Delegates;
using Kinect_Center.Controller;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.Controls;
using MyComponents.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kinect_Center.View
{
    /// <summary>
    /// Interaction logic for Arkanoid.xaml
    /// </summary>
    public partial class Arkanoid : ContentForm
    {
        #region [Atributos]


        #region [Atributos game]
        Point ballPosition = new Point(0, 0);
        Vector ballDirection = new Vector(2, 2);
        BeamManager _beamManager;
        double padPosition = 0;
        double padDirection = 0;
        const double padInertia = 0.80;
        const double padSpeed = 2;
        int score = 0;
        #endregion [Atributos game]

        #region [Atributos componente audio]

        /// <summary>
        /// Number of milliseconds between each read of audio data from the stream.
        /// Faster polling (few tens of ms) ensures a smoother audio stream visualization.
        /// </summary>
        private const int AudioPollingInterval = 50;

        /// <summary>
        /// Number of samples captured from Kinect audio stream each millisecond.
        /// </summary>
        private const int SamplesPerMillisecond = 16;

        /// <summary>
        /// Number of bytes in each Kinect audio stream sample.
        /// </summary>
        private const int BytesPerSample = 2;

        /// <summary>
        /// Number of audio samples represented by each column of pixels in wave bitmap.
        /// </summary>
        private const int SamplesPerColumn = 40;

        /// <summary>
        /// Width of bitmap that stores audio stream energy data ready for visualization.
        /// </summary>
        private const int EnergyBitmapWidth = 780;

        /// <summary>
        /// Height of bitmap that stores audio stream energy data ready for visualization.
        /// </summary>
        private const int EnergyBitmapHeight = 195;

        /// <summary>
        /// Bitmap that contains constructed visualization for audio stream energy, ready to
        /// be displayed. It is a 2-color bitmap with white as background color and blue as
        /// foreground color.
        /// </summary>
        private WriteableBitmap energyBitmap;

        /// <summary>
        /// Rectangle representing the entire energy bitmap area. Used when drawing background
        /// for energy visualization.
        /// </summary>
        private readonly Int32Rect fullEnergyRect = new Int32Rect(0, 0, EnergyBitmapWidth, EnergyBitmapHeight);

        /// <summary>
        /// Array of background-color pixels corresponding to an area equal to the size of whole energy bitmap.
        /// </summary>
        private readonly byte[] backgroundPixels = new byte[EnergyBitmapWidth * EnergyBitmapHeight];

        /// <summary>
        /// Buffer used to hold audio data read from audio stream.
        /// </summary>
        private readonly byte[] audioBuffer = new byte[AudioPollingInterval * SamplesPerMillisecond * BytesPerSample];

        /// <summary>
        /// Buffer used to store audio stream energy data as we read audio.
        /// 
        /// We store 25% more energy values than we strictly need for visualization to allow for a smoother
        /// stream animation effect, since rendering happens on a different schedule with respect to audio
        /// capture.
        /// </summary>
        private readonly double[] energy = new double[(uint)(EnergyBitmapWidth * 1.25)];

        /// <summary>
        /// Object for locking energy buffer to synchronize threads.
        /// </summary>
        private readonly object energyLock = new object();

        /// <summary>
        /// Stream of audio being captured by Kinect sensor.
        /// </summary>
        private Stream audioStream;

        /// <summary>
        /// <code>true</code> if audio is currently being read from Kinect stream, <code>false</code> otherwise.
        /// </summary>
        private bool reading;

        /// <summary>
        /// Thread that is reading audio from Kinect stream.
        /// </summary>
        private Thread readingThread;

        /// <summary>
        /// Array of foreground-color pixels corresponding to a line as long as the energy bitmap is tall.
        /// This gets re-used while constructing the energy visualization.
        /// </summary>
        private byte[] foregroundPixels;

        /// <summary>
        /// Sum of squares of audio samples being accumulated to compute the next energy value.
        /// </summary>
        private double accumulatedSquareSum;

        /// <summary>
        /// Number of audio samples accumulated so far to compute the next energy value.
        /// </summary>
        private int accumulatedSampleCount;

        /// <summary>
        /// Index of next element available in audio energy buffer.
        /// </summary>
        private int energyIndex;

        /// <summary>
        /// Number of newly calculated audio stream energy values that have not yet been
        /// displayed.
        /// </summary>
        private int newEnergyAvailable;

        /// <summary>
        /// Error between time slice we wanted to display and time slice that we ended up
        /// displaying, given that we have to display in integer pixels.
        /// </summary>
        private double energyError;

        /// <summary>
        /// Last time energy visualization was rendered to screen.
        /// </summary>
        private DateTime? lastEnergyRefreshTime;

        /// <summary>
        /// Index of first energy element that has never (yet) been displayed to screen.
        /// </summary>
        private int energyRefreshIndex;
        #endregion [Atributos componente audio]


        #endregion [Atributos]

        #region [Propriedades]

        public BeamManager BeamManager
        {
            get { return _beamManager; }
            set
            {
                _beamManager = value;
            }
        }

        #endregion [Propriedades]

        #region [Construtores]

        public Arkanoid()
        {
            InitializeComponent();
            InitializeKinectCommands();
        }

        #endregion [Construtores]

        #region [Eventos]

        public event GameEnd GameEnd;

        #endregion [Eventos]

        #region [Métodos Privados]

        private void StartGame()
        {
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            UpdateGame();
            UpdateAudioComponent();
        }

        private void UpdateAudioComponent()
        {
            lock (this.energyLock)
            {
                // Calculate how many energy samples we need to advance since the last update in order to
                // have a smooth animation effect
                DateTime now = DateTime.UtcNow;
                DateTime? previousRefreshTime = this.lastEnergyRefreshTime;
                this.lastEnergyRefreshTime = now;

                // No need to refresh if there is no new energy available to render
                if (this.newEnergyAvailable <= 0)
                {
                    return;
                }

                if (previousRefreshTime != null)
                {
                    double energyToAdvance = this.energyError + (((now - previousRefreshTime.Value).TotalMilliseconds * SamplesPerMillisecond) / SamplesPerColumn);
                    int energySamplesToAdvance = Math.Min(this.newEnergyAvailable, (int)Math.Round(energyToAdvance));
                    this.energyError = energyToAdvance - energySamplesToAdvance;
                    this.energyRefreshIndex = (this.energyRefreshIndex + energySamplesToAdvance) % this.energy.Length;
                    this.newEnergyAvailable -= energySamplesToAdvance;
                }

                // clear background of energy visualization area
                this.energyBitmap.WritePixels(fullEnergyRect, this.backgroundPixels, EnergyBitmapWidth, 0);

                // Draw each energy sample as a centered vertical bar, where the length of each bar is
                // proportional to the amount of energy it represents.
                // Time advances from left to right, with current time represented by the rightmost bar.
                int baseIndex = (this.energyRefreshIndex + this.energy.Length - EnergyBitmapWidth) % this.energy.Length;
                for (int i = 0; i < EnergyBitmapWidth; ++i)
                {
                    const int HalfImageHeight = EnergyBitmapHeight / 2;

                    // Each bar has a minimum height of 1 (to get a steady signal down the middle) and a maximum height
                    // equal to the bitmap height.
                    int barHeight = (int)Math.Max(1.0, (this.energy[(baseIndex + i) % this.energy.Length] * EnergyBitmapHeight));

                    // Center bar vertically on image
                    var barRect = new Int32Rect(i, HalfImageHeight - (barHeight / 2), 1, barHeight);

                    // Draw bar in foreground color
                    this.energyBitmap.WritePixels(barRect, foregroundPixels, 1, 0);
                }
            }
        }

        private void UpdateGame()
        {
            padDirection += BeamManager.BeamAngle * 2.0 - 1.0;
            padPosition += padDirection;
            if (padPosition < 0)
            {
                padDirection = 0;
                padPosition *= -1;
            }
            else if (padPosition > playground.RenderSize.Width - paddle.Width - 1)
            {
                padPosition = playground.RenderSize.Width - paddle.Width - 1;
                padDirection *= -1;
            }

            padDirection *= padInertia;


            // Ball
            ballPosition += ballDirection;

            // Walls
            if (ballPosition.X < 0)
            {
                ballPosition.X = 0;
                ballDirection.X *= -1;
            }
            else if (ballPosition.X >= playground.RenderSize.Width - ball.Width)
            {
                ballPosition.X = playground.RenderSize.Width - ball.Width - 1;
                ballDirection.X *= -1;
            }

            if (ballPosition.Y < 0)
            {
                ballPosition.Y = 0;
                ballDirection.Y *= -1;
            }
            else if (ballPosition.Y >= playground.RenderSize.Height - ball.Height)
            {
                CompositionTarget.Rendering -= CompositionTarget_Rendering;
                scoreText.Text = "Final score: " + (score / 10).ToString();
                if (this.readingThread.IsAlive)
                    this.readingThread.Abort();
                if (GameEnd != null)
                    GameEnd();
                return;
            }

            // Collisions
            var padRect = new Rect(padPosition, playground.RenderSize.Height - 50, paddle.Width, paddle.Height);
            var ballRect = new Rect(ballPosition, new Size(ball.Width, ball.Height));

            if (padRect.IntersectsWith(ballRect))
            {
                ballPosition.Y = playground.RenderSize.Height - 50 - ball.Height;
                ballDirection.Y *= -1;
            }

            // Moving
            Canvas.SetLeft(ball, ballPosition.X);
            Canvas.SetTop(ball, ballPosition.Y);

            Canvas.SetTop(paddle, playground.RenderSize.Height - 50);
            Canvas.SetLeft(paddle, padPosition);

            // Score
            scoreText.Text = (score / 10).ToString();
            score++;
        }

        /// <summary>
        /// Handles polling audio stream and updating visualization every tick.
        /// </summary>
        private void AudioReadingThread()
        {
            // Bottom portion of computed energy signal that will be discarded as noise.
            // Only portion of signal above noise floor will be displayed.
            const double EnergyNoiseFloor = 0.2;

            while (this.reading)
            {
                int readCount = audioStream.Read(audioBuffer, 0, audioBuffer.Length);

                // Calculate energy corresponding to captured audio.
                // In a computationally intensive application, do all the processing like
                // computing energy, filtering, etc. in a separate thread.
                lock (this.energyLock)
                {
                    for (int i = 0; i < readCount; i += 2)
                    {
                        // compute the sum of squares of audio samples that will get accumulated
                        // into a single energy value.
                        short audioSample = BitConverter.ToInt16(audioBuffer, i);
                        this.accumulatedSquareSum += audioSample * audioSample;
                        ++this.accumulatedSampleCount;

                        if (this.accumulatedSampleCount < SamplesPerColumn)
                        {
                            continue;
                        }

                        // Each energy value will represent the logarithm of the mean of the
                        // sum of squares of a group of audio samples.
                        double meanSquare = this.accumulatedSquareSum / SamplesPerColumn;
                        double amplitude = Math.Log(meanSquare) / Math.Log(int.MaxValue);

                        // Renormalize signal above noise floor to [0,1] range.
                        this.energy[this.energyIndex] = Math.Max(0, amplitude - EnergyNoiseFloor) / (1 - EnergyNoiseFloor);
                        this.energyIndex = (this.energyIndex + 1) % this.energy.Length;

                        this.accumulatedSquareSum = 0;
                        this.accumulatedSampleCount = 0;
                        ++this.newEnergyAvailable;
                    }
                }
            }
        }

        private void InitializeKinectCommands()
        {
            BitmapImage image = new BitmapImage(new Uri("/Kinect Center;component/Resources/Icons/restart.png", UriKind.Relative));
            this.commandsBar.CommandBack.Click += btnBack_Click;
            this.commandsBar.AddCommand("Restart", btnRestart_Click, image);
        }

        #endregion [Métodos Privados]

        private void ContentForm_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeAudioComponent();
            RestartGame();
        }

        private void InitializeAudioComponent()
        {
            this.energyBitmap = new WriteableBitmap(EnergyBitmapWidth, EnergyBitmapHeight, 96, 96, PixelFormats.Indexed1, new BitmapPalette(new List<Color> { Colors.White, Colors.Blue }));
            // Initialize foreground pixels
            this.foregroundPixels = new byte[EnergyBitmapHeight];
            for (int i = 0; i < this.foregroundPixels.Length; ++i)
            {
                this.foregroundPixels[i] = 0xff;
            }

            this.waveDisplay.Source = this.energyBitmap;

            // Start streaming audio!
            this.audioStream = FrontController.Instance.KinectSensorManager.KinectAudioStream;

            // Use a separate thread for capturing audio because audio stream read operations
            // will block, and we don't want to block main UI thread.
            this.reading = true;
            this.readingThread = new Thread(AudioReadingThread);
            this.readingThread.Start();
        }

        public void RestartGame()
        {
            Canvas.SetTop(ball, 0);
            Canvas.SetLeft(ball, 0);
            ballPosition.X = 0;
            ballPosition.Y = 0;
            padPosition = 0;
            Canvas.SetLeft(paddle, 0);
            padDirection = 0;
            score = 0;
            StartGame();
            FrontController.Instance.SpeechBar.HideSpeechBar();
            HideCommandsBar();
            if (!this.readingThread.IsAlive)
            {
                this.readingThread = new Thread(AudioReadingThread);
                this.readingThread.Start();
            }

        }

        public void ShowCommandsBar()
        {
            this.kinectRegion.Visibility = Visibility.Visible;
            FrontController.Instance.KinectSensorManager.BindingKinectRegion(this.kinectRegion, KinectRegion.KinectSensorProperty);
        }

        public void HideCommandsBar()
        {
            this.kinectRegion.Visibility = Visibility.Collapsed;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            FrontController.Instance.BackToHome();
        }

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            RestartGame();
        }

        private void ContentForm_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.readingThread.IsAlive)
                this.readingThread.Abort();
        }

        internal void UpdateFont(System.Windows.Media.FontFamily fontFamily)
        {
            commandsBar.UpdateFont(fontFamily);
        }

    }
}
