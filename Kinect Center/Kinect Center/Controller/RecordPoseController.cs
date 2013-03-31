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
using Kinect_Center.UIFunctions;

namespace Kinect_Center.Controller
{
    public class RecordPoseController : FormControllerBase, IKinectFunctionsController, ISpeechBarController
    {
        #region [Atributos]

        private int count;

        #endregion [Atributos]

        #region [Construtores]

        public RecordPoseController()
        {
            RecordPose view = new RecordPose();
            this.Form = view;
            this.AcceptableCommands = CreateCommands();
            InitializeButtonEvents(view);
        }

        #endregion [Construtores]

        #region [Métodos implementados das interfaces]

        public virtual void InitializeKinectFunctions(Business.Classes.KinectSensorManager kinectManager)
        {
            kinectManager.Kinect.SkeletonFrameReady += Kinect_SkeletonFrameReady;
        }



        public virtual void DisposeKinectFunctions(Business.Classes.KinectSensorManager kinectManager)
        {
            kinectManager.Kinect.SkeletonFrameReady -= Kinect_SkeletonFrameReady;
        }

        public void SpeechRecognized(string command, double confidenceLevel)
        {
            switch (command)
            {
                case "Back":
                    FrontController.Instance.BackToHome(confidenceLevel);
                    break;
                case "Record":
                    RecordCurrentPose();
                    break;
                case "Clear":
                    GetView<RecordPose>().ClearCurrentPose();
                    break;
                case "Compose":
                    GoToComposeView();
                    break;
            }
        }

        public void OnSpeechBarStatusChanged(bool isActivated)
        {
            if (isActivated)
                this.GetView<RecordPose>().UpdateFont(new FontFamily("Segoe UI Semibold"));
            else
                this.GetView<RecordPose>().UpdateFont(new FontFamily("Segoe UI Light"));
        }


        public List<string> AcceptableCommands
        {
            get;
            private set;
        }
        #endregion [Métodos Sobrescritos da Herança]

        #region [Métodos privados]

        private void InitializeButtonEvents(RecordPose view)
        {
            view.AddRecordPoseCommand(btnRecordPose_Click);
            view.AddComposeCommand(btnCompose_Click);
            view.AddClearPoseCommand(btnClearPose_Click);
        }

        private List<string> CreateCommands()
        {
            List<string> commands = new List<string>();
            commands.Add("Back");
            commands.Add("Record");
            commands.Add("Clear");
            commands.Add("Compose");

            return commands;
        }

        private void Kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame currentFrame = e.OpenSkeletonFrame())
            {
                RecordPose view = GetView<RecordPose>();
                view.CurrentUserSkeleton = FrontController.Instance.KinectSensorManager.GetFirstSkeletonFromSkeletonFrame(currentFrame);
                view.currentSkeletonImage.Children.Clear();
                if (view.CurrentUserSkeleton != null)
                    UIFunctionsManager.DrawUserSkeleton(view.CurrentUserSkeleton, view.currentSkeletonImage, true, true);
            }
        }

        private void btnRecordPose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            RecordCurrentPose();
        }

        private void btnCompose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GoToComposeView();
        }

        private static void GoToComposeView()
        {
            if (FrontController.Instance.CurrentPoseDataContext != null &&
                FrontController.Instance.CurrentPoseDataContext.CurrentPose != null)
            {
                FrontController.Instance.ChangeCurrentController<ComposePoseController>();
            }
            else
            {
                UIFunctionsManager.ShowAlert("Invalid Operation", "You must have recorded a pose to go to the next step");
            }
        }

        private void btnClearPose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GetView<RecordPose>().ClearCurrentPose();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            count--;

            if (count == 0)
            {
                RecordPose view = GetView<RecordPose>();
                view.Timer.Stop();
                view.Timer.Tick -= timer_Tick;
                view.lbContador.Visibility = System.Windows.Visibility.Hidden;
                view.RecordCurrentPose();
            }
            else
            {
                GetView<RecordPose>().lbContador.Content = count.To<string>();
            }
        }

        private void RecordCurrentPose()
        {
            RecordPose view = GetView<RecordPose>();
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
