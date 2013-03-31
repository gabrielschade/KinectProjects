using Kinect_Center.Business.Delegates;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Kinect_Center.Business.Classes
{
    public class KinectSensorManager
    {

        #region [Atributos]

        private SpeechRecognitionEngine _kinectSpeechRecognitionEngine;

        #endregion [Atributos]

        #region [Propriedades]

        public KinectSensorChooser KinectChooser { get; private set; }

        public KinectSensor Kinect
        {
            get { return KinectChooser.Kinect; }
        }

        public Stream KinectAudioStream { get; private set; }

        public KinectAudioSource KinectAudioSource
        {
            get;
            private set;
        }

        #endregion [Propriedades]

        #region [Construtores]

        public KinectSensorManager()
        {
            this.KinectChooser = new KinectSensorChooser();
            this.KinectChooser.KinectChanged += SensorChooserOnKinectChanged;
            this.KinectChooser.Start();
        }

        #endregion [Construtores]

        #region [Eventos]

        public event SpeechRecognized SpeechRecognized;

        #endregion [Eventos]

        #region [Métodos Públicos]

        /// <summary>
        /// Verificar este método para explicação na palestra
        /// </summary>
        public void InitializeSpeechRecognition()
        {
            Func<RecognizerInfo, bool> matchingFunc = recognizer =>
            {
                string value;
                recognizer.AdditionalInfo.TryGetValue("Kinect", out value);
                return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase)
                && "en-US".Equals(recognizer.Culture.Name, StringComparison.InvariantCultureIgnoreCase);
            };

            RecognizerInfo recognizerInfo = SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).FirstOrDefault();

            Choices commands = new Choices(GenerateAllVoiceCommands());

            GrammarBuilder grammarBuilder = new GrammarBuilder(commands);
            grammarBuilder.Culture = recognizerInfo.Culture;

            Grammar gramar = new Grammar(grammarBuilder);

            _kinectSpeechRecognitionEngine = new SpeechRecognitionEngine(recognizerInfo.Id);
            _kinectSpeechRecognitionEngine.LoadGrammar(gramar);
            _kinectSpeechRecognitionEngine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(KinectSpeechRecognitionEngine_SpeechRecognized);

            KinectAudioSource = this.Kinect.AudioSource;
            KinectAudioSource.AutomaticGainControlEnabled = true;
            KinectAudioSource.EchoCancellationMode = EchoCancellationMode.CancellationAndSuppression;
            KinectAudioStream = KinectAudioSource.Start();
            _kinectSpeechRecognitionEngine.SetInputToAudioStream(KinectAudioStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
            _kinectSpeechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
        }

        /// <summary>
        /// Vincula um kinectRegion com o sensor kinect do manager
        /// </summary>
        /// <param name="kinectRegion"></param>
        /// <param name="kinectSensorProperty"></param>
        public void BindingKinectRegion(DependencyObject kinectRegion, DependencyProperty kinectSensorProperty)
        {
            Binding regionSensorBinding = new Binding("Kinect") { Source = this };
            BindingOperations.SetBinding(kinectRegion, kinectSensorProperty, regionSensorBinding);
        }

        /// <summary>
        /// Obtém o esqueleto mais próximo do sensor
        /// </summary>
        /// <param name="currentFrame">frame atual do stream de esqueleto</param>
        /// <returns>objeto referente ao esqueleto do usuário</returns>
        public Skeleton GetFirstSkeletonFromSkeletonFrame(SkeletonFrame currentFrame)
        {
            if (currentFrame == null)
                return null;

            Skeleton[] usersSkeleton = new Skeleton[6];
            Skeleton skeletonReturn = null;
            currentFrame.CopySkeletonDataTo(usersSkeleton);
            IEnumerable<Skeleton> trackedSkeleton = usersSkeleton.Where(skeleton => skeleton.TrackingState == SkeletonTrackingState.Tracked);
            if (trackedSkeleton.Count() > 0)
            {
                skeletonReturn = trackedSkeleton.First();
            }

            return skeletonReturn;
        }

        #endregion [Métodos Públicos]

        #region [Métodos Privados]

        private string[] GenerateAllVoiceCommands()
        {
            List<string> commands = new List<string>();

            //General Commands
            commands.Add("Kinect");
            commands.Add("Cancel");
            commands.Add("Back");

            //Home Commands
            commands.Add("Arkanoid");
            commands.Add("Instant Cam");
            commands.Add("InfraRed Cam");
            commands.Add("Depth Cam");
            commands.Add("Record Pose");

            //Arkanoid Commands
            commands.Add("Restart");

            //Camera Commands
            commands.Add("Picture");

            //Pose Commands
            commands.Add("Record");
            commands.Add("Clear");
            commands.Add("Compose");
            commands.Add("Test");
            commands.Add("Home");

            return commands.ToArray();
        }

        private void KinectSpeechRecognitionEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (SpeechRecognized != null)
                SpeechRecognized(e.Result.Text, e.Result.Confidence);
        }

        /// <summary>
        /// Chamado quando o KinectSensorChooser obtém um novo sensor
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="kinectArgs">argumentos</param>
        private void SensorChooserOnKinectChanged(object sender, KinectChangedEventArgs kinectArgs)
        {
            if (kinectArgs.OldSensor != null)
            {
                try
                {
                    if (kinectArgs.OldSensor.DepthStream.IsEnabled)
                        kinectArgs.OldSensor.DepthStream.Disable();

                    if (kinectArgs.OldSensor.SkeletonStream.IsEnabled)
                        kinectArgs.OldSensor.SkeletonStream.Disable();

                    if (kinectArgs.OldSensor.ColorStream.IsEnabled)
                        kinectArgs.OldSensor.ColorStream.Disable();
                }
                catch (InvalidOperationException)
                {
                    // Captura exceção caso o KinectSensor entre em um estado inválido durante a desabilitação das streams.
                }
            }

            if (kinectArgs.NewSensor != null)
            {
                try
                {
                    kinectArgs.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                    kinectArgs.NewSensor.SkeletonStream.Enable();

                    // dispositivos Kinect para xbox nao podem utilizar o near Range então deve-se setar estas propriedades.
                    kinectArgs.NewSensor.DepthStream.Range = DepthRange.Default;
                    kinectArgs.NewSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    InitializeSpeechRecognition();
                }
                catch (InvalidOperationException)
                {
                    // Captura exceção caso o KinectSensor entre em um estado inválido durante a desabilitação das streams.
                }
            }
        }

        #endregion [Métodos Privados]
    }
}
