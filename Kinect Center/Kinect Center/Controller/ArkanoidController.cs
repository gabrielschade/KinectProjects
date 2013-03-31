using Kinect_Center.Business.Classes;
using Kinect_Center.Business.Interfaces;
using Kinect_Center.View;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Kinect_Center.Controller
{
    public class ArkanoidController : FormControllerBase, ISpeechBarController, IKinectFunctionsController
    {

        #region [Atributos]

        BeamManager _beamManager;

        #endregion [Atributos]

        #region [Construtores]

        public ArkanoidController()
        {
            Arkanoid view = new Arkanoid();
            view.GameEnd += view_GameEnd;
            this.Form = view;
            this.AcceptableCommands = CreateCommands();
        }

        #endregion [Construtores]

        #region [Métodos implementados das interfaces]

        public void InitializeKinectFunctions(Business.Classes.KinectSensorManager kinectManager)
        {
            _beamManager = new BeamManager(kinectManager.KinectAudioSource);
            Arkanoid view = this.GetView<Arkanoid>();
            view.BeamManager = _beamManager;
            kinectManager.KinectAudioSource.BeamAngleChanged += this.AudioSourceBeamChanged;
            kinectManager.KinectAudioSource.SoundSourceAngleChanged += this.AudioSourceSoundSourceAngleChanged;

        }

        public void DisposeKinectFunctions(Business.Classes.KinectSensorManager kinectManager)
        {
            _beamManager.Dispose();
            kinectManager.KinectAudioSource.BeamAngleChanged -= this.AudioSourceBeamChanged;
            kinectManager.KinectAudioSource.SoundSourceAngleChanged -= this.AudioSourceSoundSourceAngleChanged;
        }

        public void SpeechRecognized(string command, double confidenceLevel)
        {
            switch (command)
            {
                case "Restart":
                    this.GetView<Arkanoid>().RestartGame();
                    break;
                case "Back":
                    FrontController.Instance.BackToHome(confidenceLevel);
                    break;
            }
        }

        public void OnSpeechBarStatusChanged(bool isActivated)
        {
            if (isActivated)
                this.GetView<Arkanoid>().UpdateFont(new FontFamily("Segoe UI Semibold"));
            else
                this.GetView<Arkanoid>().UpdateFont(new FontFamily("Segoe UI Light"));
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
            commands.Add("Restart");
            commands.Add("Back");

            return commands;
        }

        private void view_GameEnd()
        {
            GetView<Arkanoid>().ShowCommandsBar();
            FrontController.Instance.ShowSpeechBar();
        }

        /// <summary>
        /// Handles event triggered when audio beam angle changes.
        /// </summary>
        /// <param name="sender">object sending the event.</param>
        /// <param name="e">event arguments.</param>
        private void AudioSourceBeamChanged(object sender, BeamAngleChangedEventArgs e)
        {

            GetView<Arkanoid>().beamRotation.Angle = -e.Angle;
        }

        /// <summary>
        /// Handles event triggered when sound source angle changes.
        /// </summary>
        /// <param name="sender">object sending the event.</param>
        /// <param name="e">event arguments.</param>
        private void AudioSourceSoundSourceAngleChanged(object sender, SoundSourceAngleChangedEventArgs e)
        {
            Arkanoid view = GetView<Arkanoid>();
            // Maximum possible confidence corresponds to this gradient width
            const double MinGradientWidth = 0.04;

            // Set width of mark based on confidence.
            // A confidence of 0 would give us a gradient that fills whole area diffusely.
            // A confidence of 1 would give us the narrowest allowed gradient width.
            double halfWidth = Math.Max((1 - e.ConfidenceLevel), MinGradientWidth) / 2;

            // Update the gradient representing sound source position to reflect confidence
            view.sourceGsPre.Offset = Math.Max(view.sourceGsMain.Offset - halfWidth, 0);
            view.sourceGsPost.Offset = Math.Min(view.sourceGsMain.Offset + halfWidth, 1);

            // Rotate gradient to match angle
            view.sourceRotation.Angle = -e.Angle;
        }

        #endregion [Métodos Privados]





    }
}
