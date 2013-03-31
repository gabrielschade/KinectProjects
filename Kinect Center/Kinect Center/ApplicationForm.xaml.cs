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
    public partial class ApplicationForm : MyComponents.Forms.MetroForm
    {
        #region [Propriedades]

        public SpeechBar SpeechBar 
        {
            get { return this.speechBar; } 
        }

        public NavigationPanel NavigationPanel 
        {
            get { return this.navigationPanel; } 
        }

        #endregion [Propriedades]


        public ApplicationForm()
        {
            InitializeComponent();
            FrontController.Instance.InitializeArchitecture(this);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            FrontController.Instance.BackToHome(1);
        }
    }
}
