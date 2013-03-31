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
using MyLibrary.Util;
using MyKinectLibrary.Util;
using MyKinectLibrary.Model;

namespace Kinect_Center.View
{
    /// <summary>
    /// Interaction logic for RecordPose.xaml
    /// </summary>
    public partial class ComposePose : ContentForm
    {

        #region [Atributos]

        private Joint? _auxiliaryJoint1;
        private Joint? _auxiliaryJoint2;
        private Joint? _centerJoint;
        private List<KinectTileButton> _disabledButtons;

        #endregion [Atributos]

        #region [Propriedades]

        public Skeleton CurrentUserSkeleton { get; set; }

        #endregion [Propriedades]

        #region [Constructors]

        /// <summary>
        /// Default Constructor that sets the setting content form
        /// </summary>
        public ComposePose()
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

        public void AddClearPoseCommand(RoutedEventHandler btnClearPose_Click)
        {
            this.commandsBar.AddCommand("Clear", btnClearPose_Click, new BitmapImage(new Uri("/Kinect Center;component/Resources/Icons/recycle.png", UriKind.Relative)));
        }

        public void AddTestCommand(RoutedEventHandler btnCompose_Click)
        {
            this.commandsBar.AddCommand("Test", btnCompose_Click, new BitmapImage(new Uri("/Kinect Center;component/Resources/Icons/next.png", UriKind.Relative)));
        }

        public void ClearSubPoses()
        {
            pnlSubPoses.Children.Clear();
            FrontController.Instance.CurrentPoseDataContext.CurrentPose.SubPoses.Clear();
        }

        #endregion [Métodos Públicos]

        #region [Métodos Privados]

        private void InitializeKinectRegion()
        {
            Binding regionSensorBinding = new Binding("Kinect") { Source = FrontController.Instance.KinectSensorManager };
            BindingOperations.SetBinding(this.kinectRegion, KinectRegion.KinectSensorProperty, regionSensorBinding);

            this.commandsBar.CommandBack.Click += CommandBack_Click;
            _disabledButtons = new List<KinectTileButton>();
        }

        private void CommandBack_Click(object sender, RoutedEventArgs e)
        {
            FrontController.Instance.ChangeCurrentController<RecordPoseController>(true);
        }

        private void ContentForm_Loaded(object sender, RoutedEventArgs e)
        {
            currentSkeletonImage.Children.Clear();
            if (FrontController.Instance.CurrentPoseDataContext.CurrentUserSkeleton != null)
            {
                UIFunctionsManager.DrawUserSkeleton(FrontController.Instance.CurrentPoseDataContext.CurrentUserSkeleton, currentSkeletonImage, true, true);
            }
        }

        private void KinectTileButtonClick(object sender, RoutedEventArgs e)
        {
            KinectTileButton button = e.OriginalSource as KinectTileButton;
            button.IsEnabled = false;
            button.Background = new SolidColorBrush(Color.FromRgb((byte)154, (byte)207, (byte)230));
            _disabledButtons.Add(button);
            JointType jointType = (JointType)button.Tag.To<int>();
            SelectJoint(jointType);

        }

        private void SelectJoint(JointType jointType)
        {
            Ellipse joint = (Ellipse)currentSkeletonImage.Children.Cast<UIElement>()
                                                                   .First(control => control is Ellipse
                                                                   && ((Ellipse)control).Tag.To<int>() == jointType.To<int>());

            joint.Stroke = Brushes.White;
            if (!_auxiliaryJoint1.HasValue)
            {
                _auxiliaryJoint1 = FrontController.Instance.CurrentPoseDataContext.CurrentUserSkeleton.Joints[jointType];
            }
            else if (!_centerJoint.HasValue)
            {
                _centerJoint = FrontController.Instance.CurrentPoseDataContext.CurrentUserSkeleton.Joints[jointType];
            }
            else if (!_auxiliaryJoint2.HasValue)
            {
                FrontController controller = FrontController.Instance;
                _auxiliaryJoint2 = controller.CurrentPoseDataContext.CurrentUserSkeleton.Joints[jointType];
                controller.CurrentPoseDataContext.CurrentPose.SubPoses.Add(new SubPose(_centerJoint.Value, _auxiliaryJoint1.Value, _auxiliaryJoint2.Value));

                CreateLabelSubPoses(_auxiliaryJoint1.Value, _centerJoint.Value, _auxiliaryJoint2.Value);

                _auxiliaryJoint1 = null;
                _auxiliaryJoint2 = null;
                _centerJoint = null;

                foreach (KinectTileButton tileButton in _disabledButtons)
                {
                    tileButton.IsEnabled = true;
                    tileButton.Background = new SolidColorBrush(Color.FromRgb((byte)65, (byte)177, (byte)225));
                }

                _disabledButtons.Clear();
                currentSkeletonImage.Children.Clear();
                UIFunctionsManager.DrawUserSkeleton(controller.CurrentPoseDataContext.CurrentUserSkeleton, currentSkeletonImage, true, true);
            }

        }

        private void CreateLabelSubPoses(Joint _auxiliaryJoint1, Joint _centerJoint, Joint _auxiliaryJoint2)
        {
            Label lbSubPose = new Label();

            StringBuilder textBuilder = new StringBuilder();
            textBuilder.AppendFormat("SubPose {0}: ", pnlSubPoses.Children.Count + 1);
            textBuilder.AppendLine();
            textBuilder.AppendFormat("Auxiliary 1: {0}", _auxiliaryJoint1.JointType.ToText());
            textBuilder.AppendLine();
            textBuilder.AppendFormat("Center 1: {0}", _centerJoint.JointType.ToText());
            textBuilder.AppendLine();
            textBuilder.AppendFormat("Auxiliary 2: {0}", _auxiliaryJoint2.JointType.ToText());
            textBuilder.AppendLine();
            lbSubPose.Content = textBuilder.ToString();
            lbSubPose.FontFamily = new System.Windows.Media.FontFamily("Segoe UI Light");
            lbSubPose.FontSize = 30;
            lbSubPose.Foreground = Brushes.Black;

            pnlSubPoses.Children.Add(lbSubPose);
            pnlSubPoses.Height = Math.Max(300, pnlSubPoses.Children.Count * 250);
        }


        #endregion [Métodos Privados]




    }
}
