using Kinect_Center.Business.Classes;
using Kinect_Center.Business.Enums;
using Kinect_Center.Business.Interfaces;
using Kinect_Center.View;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kinect_Center.Controller
{
    public class HomeController : FormControllerBase, ISpeechBarController
    {

        #region [Construtores]

        public HomeController()
        {
            Home view = new Home();
            this.Form = view;
            this.AcceptableCommands = CreateCommands();
            InitializeButtonEvents(view);

        }

        #endregion [Construtores]

        #region [Métodos implementados da interfaces]

        public void SpeechRecognized(string command, double confidenceLevel)
        {
            switch (command)
            {
                case "Arkanoid":
                    OpenController<ArkanoidController>();
                    break;
                case "Instant Cam":
                    OpenController<InstantCamController>();
                    break;
                case "InfraRed Cam":
                    OpenController<InfraRedCamController>();
                    break;
                case "Depth Cam":
                    OpenController<DepthCamController>();
                    break;
                case "Record Pose":
                    OpenController<RecordPoseController>();
                    break;
            }
        }

        public void OnSpeechBarStatusChanged(bool isActivated)
        {
            System.Windows.Media.FontFamily font;
            Home view = GetView<Home>();
            if (isActivated)
            {
                font = new System.Windows.Media.FontFamily("Segoe UI Semibold");
                view.txtSpeech.Text = "Say \"Cancel\" to deactivate speech recognition";
            }
            else
            {
                font = new System.Windows.Media.FontFamily("Segoe UI Light");
                view.txtSpeech.Text = "Say \"Kinect\"   to activate speech recognition";
            }
            view.UpdateFont(font);
        }

        public List<string> AcceptableCommands
        {
            get;
            private set;
        }

        #endregion [Métodos implementados das interfaces]

        #region [Métodos Privados]

        private List<string> CreateCommands()
        {
            List<string> commands = new List<string>();
            commands.Add("Arkanoid");
            commands.Add("Instant Cam");
            commands.Add("InfraRed Cam");
            commands.Add("Depth Cam");
            commands.Add("Record Pose");
            return commands;
        }

        private void OpenController<T>() where T : FormControllerBase
        {
            FrontController.Instance.ChangeCurrentController<T>();
        }

        private void InitializeButtonEvents(Home view)
        {
            view.btnArkanoid.Click += btnArkanoid_Click;
            view.btnInstantCam.Click += btnInstantCam_Click;
            view.btnSpeech.Click += btnSpeech_Click;
            view.btnInfraRedCam.Click += btnInfraRedCam_Click;
            view.btnDepthCam.Click += btnDepthCam_Click;
            view.btnRecordPose.Click += btnRecordPose_Click;
        }
       

        #endregion [Métodos Privados]

        #region [Métodos vinculados a eventos]

        private void btnArkanoid_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenController<ArkanoidController>();
        }

        private void btnInstantCam_Click(object sender, RoutedEventArgs e)
        {
            OpenController<InstantCamController>();
        }

        private void btnSpeech_Click(object sender, RoutedEventArgs e)
        {
            if (FrontController.Instance.SpeechBar.Visibility == Visibility.Visible)
                FrontController.Instance.HideSpeechBar();
            else
                FrontController.Instance.ShowSpeechBar();
        }

        private void btnInfraRedCam_Click(object sender, RoutedEventArgs e)
        {
            OpenController<InfraRedCamController>();
        }

        private void btnDepthCam_Click(object sender, RoutedEventArgs e)
        {
            OpenController<DepthCamController>();
        }

        private void btnRecordPose_Click(object sender, RoutedEventArgs e)
        {
            OpenController<RecordPoseController>();
        }

        #endregion [Métodos vinculados a eventos]


        
    }
}
