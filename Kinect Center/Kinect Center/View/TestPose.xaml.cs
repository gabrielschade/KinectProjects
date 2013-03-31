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
using MyKinectLibrary.Enums;
using Kinect_Center.Business.Classes;
using MyKinectLibrary.Model;

namespace Kinect_Center.View
{
    /// <summary>
    /// Interaction logic for RecordPose.xaml
    /// </summary>
    public partial class TestPose : ContentForm
    {

        #region [Constructors]

        /// <summary>
        /// Default Constructor that sets the setting content form
        /// </summary>
        public TestPose()
        {
            InitializeComponent();
            InitializeKinectRegion();
        }

        #endregion end of [Constructors]

        #region [Métodos Públicos]

        public void UpdateFont(System.Windows.Media.FontFamily fontFamily)
        {
            commandsBar.UpdateFont(fontFamily);
        }

        public void AddHomeCommand(RoutedEventHandler btnRecordPose_Click)
        {
            this.commandsBar.AddCommand("Home", btnRecordPose_Click, new BitmapImage(new Uri("/Kinect Center;component/Resources/Icons/home.png", UriKind.Relative)));
        }

        public void UpdateStatusText(PoseEventStatus status)
        {
            string textStatus;
            switch (status)
            {
                case PoseEventStatus.recognized:
                    textStatus = "Recognized";
                    break;
                case PoseEventStatus.interrupted:
                    textStatus = "Not Recognized";
                    break;
                case PoseEventStatus.in_progress:
                    textStatus = "In Progress";
                    break;
                default:
                    textStatus = "Unknow";
                    break;
            }

            lbCurrentStatus.Content = string.Format("Current Status: {0}", textStatus);
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
            FrontController.Instance.ChangeCurrentController<ComposePoseController>(true);
        }


        private void AddToRelevantJoints(List<JointType> listRelevantJoints, SubPose subPose)
        {
            AddRelevantJoint(listRelevantJoints, subPose.CenterJoint.JointType);
            AddRelevantJoint(listRelevantJoints, subPose.AuxiliaryJoint1.JointType);
            AddRelevantJoint(listRelevantJoints, subPose.AuxiliaryJoint2.JointType);
        }


        private void AddRelevantJoint(List<JointType> listRelevantJoints, JointType jointType)
        {
            if (!listRelevantJoints.Contains(jointType))
                listRelevantJoints.Add(jointType);
        }

        #endregion [Métodos Privados]

        private void ContentForm_Loaded(object sender, RoutedEventArgs e)
        {
            PoseDataContext pose = FrontController.Instance.CurrentPoseDataContext;
            List<JointType> listRelevantJoints = new List<JointType>();
            pose.CurrentPose.SubPoses.ForEach(subPose => AddToRelevantJoints(listRelevantJoints, subPose));
            UIFunctionsManager.DrawUserSkeleton(pose.CurrentUserSkeleton, referenceSkeletonCanvas, true, true, Brushes.DarkGray, listRelevantJoints);
        }


    }
}
