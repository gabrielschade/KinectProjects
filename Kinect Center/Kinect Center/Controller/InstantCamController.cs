using Kinect_Center.View;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using MyLibrary.Util;
using Kinect_Center.Business.Interfaces;
using System.Windows.Media;

namespace Kinect_Center.Controller
{
    public class InstantCamController : FormControllerBase, IKinectFunctionsController, ISpeechBarController
    {
        #region [Atributos]

        private int count;

        #endregion [Atributos]

        #region [Construtores]

        public InstantCamController()
        {
            Camera view = new Camera();
            this.Form = view;
            this.AcceptableCommands = CreateCommands();
            InitializeButtonEvents(view);
        }

        #endregion [Construtores]

        #region [Métodos implementados das interfaces]

        public virtual void InitializeKinectFunctions(Business.Classes.KinectSensorManager kinectManager)
        {
            kinectManager.Kinect.ColorStream.Enable();
            kinectManager.Kinect.ColorFrameReady += Kinect_ColorFrameReady;
        }

        public virtual void DisposeKinectFunctions(Business.Classes.KinectSensorManager kinectManager)
        {
            kinectManager.Kinect.ColorStream.Disable();
            kinectManager.Kinect.ColorFrameReady -= Kinect_ColorFrameReady;
        }

        public void SpeechRecognized(string command, double confidenceLevel)
        {
            switch (command)
            {
                case "Back":
                    FrontController.Instance.BackToHome(confidenceLevel);
                    break;
                case "Picture":
                    TakeaPicture();
                    break;
            }
        }

        public void OnSpeechBarStatusChanged(bool isActivated)
        {
            if (isActivated)
                this.GetView<Camera>().UpdateFont(new FontFamily("Segoe UI Semibold"));
            else
                this.GetView<Camera>().UpdateFont(new FontFamily("Segoe UI Light"));
        }


        public List<string> AcceptableCommands
        {
            get;
            private set;
        }
        #endregion [Métodos Sobrescritos da Herança]

        #region [Métodos privados]

        private void InitializeButtonEvents(Camera view)
        {
            view.AddTakeaPictureCommand(btnTakeAPicture_Click);
        }

        private List<string> CreateCommands()
        {
            List<string> commands = new List<string>();
            commands.Add("Back");
            commands.Add("Picture");
            commands.Add("Item 1");
            commands.Add("Item 2");
            commands.Add("Item 3");
            commands.Add("Item 4");
            //commands.Add("Show in gallery");

            return commands;
        }

        private void Kinect_ColorFrameReady(object sender, Microsoft.Kinect.ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame currentFrame = e.OpenColorImageFrame())
            {
                if (currentFrame != null)
                {
                    byte[] bitsFrame = new byte[currentFrame.PixelDataLength];
                    currentFrame.CopyPixelDataTo(bitsFrame);
                    BitmapSource source = BitmapImage.Create(currentFrame.Width, currentFrame.Height, 96, 96, System.Windows.Media.PixelFormats.Bgr32, null, bitsFrame, currentFrame.Width * currentFrame.BytesPerPixel);
                    this.GetView<Camera>().UpdateCanvas(source);
                }
            }
        }

        private void btnTakeAPicture_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            TakeaPicture();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            count--;

            if (count == 0)
            {
                Camera view = GetView<Camera>();
                view.Timer.Stop();
                view.Timer.Tick -= timer_Tick;
                view.lbContador.Visibility = System.Windows.Visibility.Hidden;
                view.SavePicture();
            }
            else
            {
                GetView<Camera>().lbContador.Content = count.To<string>();
            }
        }

        private void TakeaPicture()
        {
            Camera view = GetView<Camera>();
            DispatcherTimer timer = view.Timer;
            if (!timer.IsEnabled)
            {

                count = 3;
                view.lbContador.Content = count.To<string>();
                view.lbContador.Visibility = System.Windows.Visibility.Visible;
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += timer_Tick;
                timer.Start();
            }
        }

        #endregion [Métodos privados]





    }
}
