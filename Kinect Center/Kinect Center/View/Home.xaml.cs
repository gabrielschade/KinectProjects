using Kinect_Center.Business.Classes;
using Kinect_Center.Controller;
using Microsoft.Kinect.Toolkit.Controls;
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

namespace Kinect_Center.View
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : MyComponents.Controls.ContentForm
    {
        #region [Propriedades]

        public List<KinectTileButton> KinectButtons { get; private set; }

        #endregion [Propriedades]

        #region [Construtores]
        public Home()
        {
            InitializeComponent();
            InitializeKinectRegion();
            CreateButtonList();
        }

        #endregion [Construtores]

        #region [Métodos Privados]

        private void InitializeKinectRegion()
        {
            FrontController.Instance.KinectSensorManager.BindingKinectRegion(this.kinectRegion, KinectRegion.KinectSensorProperty);
        }

        private void CreateButtonList()
        {
            this.KinectButtons = new List<KinectTileButton>();
            this.KinectButtons.Add(btnArkanoid);
            this.KinectButtons.Add(btnInstantCam);
            this.KinectButtons.Add(btnInfraRedCam);
            this.KinectButtons.Add(btnDepthCam);
        }


        #endregion [Métodos Privados]

        internal void UpdateFont(System.Windows.Media.FontFamily font)
        {
            foreach (KinectTileButton button in KinectButtons)
            {
                Label lblToUpdateFont = button.Tag as Label;
                lblToUpdateFont.FontFamily = font;
            }
        }




        
    }
}
