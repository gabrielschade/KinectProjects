using Kinect_Center.Controller;
using Microsoft.Kinect.Toolkit.Controls;
using MyComponents.Controls;
using MyComponents.NavigationPanel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Kinect_Center
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ImageVisualizer : MyComponents.Forms.MetroForm
    {
        public ImageVisualizer(ImageSource source)
        {
            InitializeComponent();
            InitializeKinectRegion();
            image.Source = source;
        }

        private void KinectCircleButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void InitializeKinectRegion()
        {
            Binding regionSensorBinding = new Binding("Kinect") { Source = FrontController.Instance.KinectSensorManager };
            BindingOperations.SetBinding(this.kinectRegion, KinectRegion.KinectSensorProperty, regionSensorBinding);
        }
    }
}
