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
    public class ComposePoseController : FormControllerBase, ISpeechBarController
    {
        #region [Construtores]

        public ComposePoseController()
        {
            ComposePose view = new ComposePose();
            this.Form = view;
            this.AcceptableCommands = CreateCommands();
            InitializeButtonEvents(view);
        }

        #endregion [Construtores]

        #region [Métodos implementados das interfaces]

        public void SpeechRecognized(string command, double confidenceLevel)
        {
            switch (command)
            {
                case "Back":
                    FrontController.Instance.ChangeCurrentController<RecordPoseController>(true);
                    break;
                case "Test":
                    //RecordCurrentPose();
                    break;
                case "Clear":
                    GetView<ComposePose>().ClearSubPoses();
                    break;
            }
        }

        public void OnSpeechBarStatusChanged(bool isActivated)
        {
            if (isActivated)
                this.GetView<ComposePose>().UpdateFont(new FontFamily("Segoe UI Semibold"));
            else
                this.GetView<ComposePose>().UpdateFont(new FontFamily("Segoe UI Light"));
        }


        public List<string> AcceptableCommands
        {
            get;
            private set;
        }
        #endregion [Métodos Sobrescritos da Herança]

        #region [Métodos privados]

        private void InitializeButtonEvents(ComposePose view)
        {
            view.AddTestCommand(btnTestPose_Click);
            view.AddClearPoseCommand(btnClearPose_Click);
        }

        private List<string> CreateCommands()
        {
            List<string> commands = new List<string>();
            commands.Add("Back");
            //commands.Add("Record");
            //commands.Add("Clear");

            return commands;
        }

        private void btnTestPose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (FrontController.Instance.CurrentPoseDataContext.CurrentPose.SubPoses.Count > 0)
            {
                FrontController.Instance.ChangeCurrentController<TestPoseController>();
            }
            else
            {
                UIFunctionsManager.ShowAlert("Invalid Operation", "You must have recorded one or more subposes to test");
            }
        }

        private void btnClearPose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GetView<ComposePose>().ClearSubPoses();
        }

        #endregion [Métodos privados]





    }
}
