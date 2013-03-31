using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading.Tasks;
using MyComponents.Controls;
using System.Windows.Threading;
using Microsoft.Kinect;
using Kinect_Center.Controller;
using MyComponents.Forms;
using Kinect_Center.UIFunctions;
using Microsoft.Kinect.Toolkit.Controls;

namespace Kinect_Center.View
{
    /// <summary>
    /// Interaction logic for RecordPose.xaml
    /// </summary>
    public partial class RecordPose : ContentForm
    {

        #region [Propriedades]

        public DispatcherTimer Timer { get; private set; }

        public Skeleton CurrentUserSkeleton { get; set; }

        #endregion [Propriedades]

        #region [Constructors]

        /// <summary>
        /// Default Constructor that sets the setting content form
        /// </summary>
        public RecordPose()
        {
            InitializeComponent();
            this.Timer = new DispatcherTimer();
            InitializeKinectRegion();
        }

        #endregion end of [Constructors]

        #region [Métodos Públicos]

        public void UpdateFont(System.Windows.Media.FontFamily fontFamily)
        {
            commandsBar.UpdateFont(fontFamily);
        }

        public void AddRecordPoseCommand(RoutedEventHandler btnRecordPose_Click)
        {
            this.commandsBar.AddCommand("Record", btnRecordPose_Click, new BitmapImage(new Uri("/Kinect Center;component/Resources/Icons/cam.png", UriKind.Relative)));
        }

        public void AddClearPoseCommand(RoutedEventHandler btnClearPose_Click)
        {
            this.commandsBar.AddCommand("Clear", btnClearPose_Click, new BitmapImage(new Uri("/Kinect Center;component/Resources/Icons/recycle.png", UriKind.Relative)));
        }

        public void AddComposeCommand(RoutedEventHandler btnCompose_Click)
        {
            this.commandsBar.AddCommand("Compose", btnCompose_Click, new BitmapImage(new Uri("/Kinect Center;component/Resources/Icons/next.png", UriKind.Relative)));
        }

        public void RecordCurrentPose()
        {
            //Verifica se o esqueleto está totalmente rastreado para evitar gravações com problemas
            if (CurrentUserSkeleton != null &&
                CurrentUserSkeleton.TrackingState == SkeletonTrackingState.Tracked &&
                     !CurrentUserSkeleton.Joints.Any(joint => joint.TrackingState != JointTrackingState.Tracked))
            {
                FrontController controller = FrontController.Instance;
                if (controller.CurrentPoseDataContext == null)
                    controller.CurrentPoseDataContext = new Business.Classes.PoseDataContext();
                if (controller.CurrentPoseDataContext.CurrentPose == null)
                    controller.CurrentPoseDataContext.CurrentPose = new MyKinectLibrary.Model.Pose();

                controller.CurrentPoseDataContext.CurrentUserSkeleton = CurrentUserSkeleton;

                controller.CurrentPoseDataContext.CurrentPose.Name = "Test Pose";
                controller.CurrentPoseDataContext.CurrentPose.WaitingTime = 30;
                controller.CurrentPoseDataContext.CurrentPose.SubPoses.Clear();

                recordedPose.Children.Clear();
                UIFunctionsManager.DrawUserSkeleton(controller.CurrentPoseDataContext.CurrentUserSkeleton, recordedPose, true, true);
            }
            else
            {
                DialogForm dialogForm = new DialogForm(MyComponents.Enums.DialogType.alert, MyComponents.Enums.DialogButtons.ok, "User Skeleton is not fully tracked", "The user skeleton must be fully tracked to record a pose.");
                dialogForm.TimeToAutoClose = 5000;
                dialogForm.ShowDialog();
            }
        }

        public void ClearCurrentPose()
        {
            FrontController controller = FrontController.Instance;
            if (controller.CurrentPoseDataContext != null)
            {
                controller.CurrentPoseDataContext.CurrentUserSkeleton = null;
                controller.CurrentPoseDataContext.CurrentPose = null;
            }
            recordedPose.Children.Clear();
        }

        #endregion [Métodos Públicos]

        #region [Métodos Privados]

        private void InitializeKinectRegion()
        {
            Binding regionSensorBinding = new Binding("Kinect") { Source = FrontController.Instance.KinectSensorManager };
            BindingOperations.SetBinding(this.kinectRegion, KinectRegion.KinectSensorProperty, regionSensorBinding);

            this.commandsBar.CommandBack.Click += CommandBack_Click;
        }

        private void CommandBack_Click(object sender, RoutedEventArgs e)
        {
            FrontController.Instance.BackToHome();
            FrontController.Instance.CurrentPoseDataContext = null;
        }

        private void ContentForm_Loaded(object sender, RoutedEventArgs e)
        {
            this.recordedPose.Children.Clear();
        }

        #endregion [Métodos Privados]


    }
}
