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
using MyKinectLibrary.PoseTracking;
using MyKinectLibrary.Enums;

namespace Kinect_Center.Controller
{
    public class TestPoseController : FormControllerBase, IKinectFunctionsController, ISpeechBarController
    {
        #region [Atributos]

        PoseRecognitionEngine _poseRecognitionEngine;
        PoseEventStatus _currentStatus = PoseEventStatus.interrupted;

        #endregion [Atributos]


        #region [Construtores]

        public TestPoseController()
        {
            TestPose view = new TestPose();
            this.Form = view;
            this.AcceptableCommands = CreateCommands();
            InitializeButtonEvents(view);
        }

        #endregion [Construtores]

        #region [Métodos implementados das interfaces]

        public virtual void InitializeKinectFunctions(Business.Classes.KinectSensorManager kinectManager)
        {
            kinectManager.Kinect.SkeletonFrameReady += Kinect_SkeletonFrameReady;
            _poseRecognitionEngine = new PoseRecognitionEngine(MyKinectLibrary.Enums.PoseRecognitionType.scalarProduct);
            _poseRecognitionEngine.PoseStatusChanged += OnPoseStatusChanged;
            _poseRecognitionEngine.Poses.Add(FrontController.Instance.CurrentPoseDataContext.CurrentPose);
        }



        public virtual void DisposeKinectFunctions(Business.Classes.KinectSensorManager kinectManager)
        {
            kinectManager.Kinect.SkeletonFrameReady -= Kinect_SkeletonFrameReady;
            _poseRecognitionEngine.PoseStatusChanged -= OnPoseStatusChanged;
            _poseRecognitionEngine = null;
        }

        public void SpeechRecognized(string command, double confidenceLevel)
        {
            switch (command)
            {
                case "Back":
                    FrontController.Instance.BackToHome(confidenceLevel);
                    break;
                case "Home":
                    GoToHome();
                    break;
            }
        }

        public void OnSpeechBarStatusChanged(bool isActivated)
        {
            if (isActivated)
                this.GetView<TestPose>().UpdateFont(new FontFamily("Segoe UI Semibold"));
            else
                this.GetView<TestPose>().UpdateFont(new FontFamily("Segoe UI Light"));
        }


        public List<string> AcceptableCommands
        {
            get;
            private set;
        }
        #endregion [Métodos Sobrescritos da Herança]

        #region [Métodos privados]

        private void InitializeButtonEvents(TestPose view)
        {
            view.AddHomeCommand(btnHome_Click);
        }

        private void btnHome_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GoToHome();
        }

        private List<string> CreateCommands()
        {
            List<string> commands = new List<string>();
            commands.Add("Back");
            commands.Add("Home");

            return commands;
        }

        private void Kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame currentFrame = e.OpenSkeletonFrame())
            {
                Skeleton userSkeleton = FrontController.Instance.KinectSensorManager.GetFirstSkeletonFromSkeletonFrame(currentFrame);
                GetView<TestPose>().currentSkeletonCanvas.Children.Clear();
                if (userSkeleton != null)
                {
                    _poseRecognitionEngine.PoseTracking(userSkeleton);
                    SolidColorBrush brushToPaint;
                    switch (_currentStatus)
                    {
                        case PoseEventStatus.recognized:
                            brushToPaint = Brushes.PaleVioletRed;
                            break;
                        case PoseEventStatus.in_progress:
                            brushToPaint = Brushes.Yellow;
                            break;
                        default:
                            brushToPaint = Brushes.LightGray;
                            break;
                    }
                    UIFunctionsManager.DrawUserSkeleton(userSkeleton, GetView<TestPose>().currentSkeletonCanvas, true, true, brushToPaint);
                }
            }
        }


        private void GoToHome()
        {
            FrontController controller = FrontController.Instance;

            controller.BackToHome();
            controller.CurrentPoseDataContext = null;
        }

        private void OnPoseStatusChanged(MyKinectLibrary.Model.Pose pose, MyKinectLibrary.Enums.PoseEventStatus status)
        {
            _currentStatus = status;
            GetView<TestPose>().UpdateStatusText(status);
        }


        #endregion [Métodos privados]





    }
}
