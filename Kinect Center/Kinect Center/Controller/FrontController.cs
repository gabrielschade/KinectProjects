using Kinect_Center.Business.Classes;
using Kinect_Center.Business.Delegates;
using Kinect_Center.Business.Interfaces;
using Microsoft.Kinect;
using MyComponents.Controls;
using MyComponents.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Center.Controller
{
    public class FrontController
    {
        #region [Atributos]

        private static FrontController _instance;
        private FormControllerBase _currentController;
        private KinectSensorManager _kinectSensorManager;
        private bool _backEffect = false;

        #endregion [Atributos]

        #region [Construtores]

        private FrontController()
        { }

        #endregion [Construtores]

        #region [Propriedades]

        public PoseDataContext CurrentPoseDataContext
        { get; set; }

        public FormControllerBase CurrentController
        {
            get { return _currentController; }
            private set
            {
                SetCurrentController(ref _currentController, value);
            }
        }

        public static FrontController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FrontController();
                return _instance;
            }
        }

        public KinectSensorManager KinectSensorManager { get { return _kinectSensorManager; } }

        public ApplicationForm ApplicationForm { get; private set; }

        public SpeechBar SpeechBar
        {
            get
            {
                return ApplicationForm.SpeechBar;
            }
        }

        #endregion [Propriedades]

        #region [Eventos]

        public event SpeechRecognized SpeechRecognized;

        public event SpeechBarStatusChanged SpeechBarStatusChanged;

        #endregion [Eventos]

        #region [Métodos Públicos]

        public void ShowSpeechBar()
        {
            this.SpeechBar.ShowSpeechBar();
            this.CallSpeechBarStatusChangedEvent(true);
        }

        public void HideSpeechBar()
        {
            this.SpeechBar.HideSpeechBar();
            this.CallSpeechBarStatusChangedEvent(false);
        }

        public void InitializeArchitecture(ApplicationForm applicationForm)
        {
            InitializeComponents(applicationForm);
            InitializeSensor();
            ChangeCurrentController<HomeController>();
        }

        public void ChangeCurrentController<T>(bool backEffect = false) where T : FormControllerBase
        {
            this._backEffect = backEffect;
            this.CurrentController = ControllerCacheManager.GetController<T>();
            if (SpeechBar.Visibility == System.Windows.Visibility.Visible)
            {
                SpeechBar.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        public void BackToHome(double confidenceLevel = 1)
        {
            if (confidenceLevel > 0.8)
            {
                ChangeCurrentController<HomeController>(true);
                CallSpeechBarStatusChangedEvent(false);
            }
        }

        #endregion [Métodos Públicos]

        #region [Métodos Privados]

        private void CallSpeechBarStatusChangedEvent(bool isActivated)
        {
            if (this.SpeechBarStatusChanged != null)
                this.SpeechBarStatusChanged(isActivated);
        }

        private void OpenForm(ContentForm form)
        {
            this.ApplicationForm.NavigationPanel.ShowForm(form);
        }

        private void BackToForm(ContentForm form)
        {
            this.ApplicationForm.NavigationPanel.BackToForm(form);
        }

        private void InitializeComponents(ApplicationForm applicationForm)
        {
            this.ApplicationForm = applicationForm;
        }

        private void InitializeSensor()
        {
            _kinectSensorManager = new KinectSensorManager();
            this.ApplicationForm.sensorChooserUi.KinectSensorChooser = _kinectSensorManager.KinectChooser;
            _kinectSensorManager.SpeechRecognized += _kinectSensorManager_SpeechRecognized;

        }

        private void SetCurrentController(ref FormControllerBase currentController, FormControllerBase newController)
        {
            if (_kinectSensorManager.Kinect != null)
            {
                if (currentController != null)
                {
                    DisposeController(ref currentController);
                }

                InitializeController(ref currentController, newController);
            }
            OpenControllerForm(newController);
        }

        private void InitializeController(ref FormControllerBase currentController, FormControllerBase newController)
        {
            currentController = newController;

            if (newController is IKinectFunctionsController)
                ((IKinectFunctionsController)newController).InitializeKinectFunctions(_kinectSensorManager);

            if (newController is ISpeechBarController)
            {
                ISpeechBarController speechBarController = newController as ISpeechBarController;

                this.SpeechRecognized += speechBarController.SpeechRecognized;
                this.SpeechBarStatusChanged += speechBarController.OnSpeechBarStatusChanged;
            }

            OpenControllerForm(newController);
        }

        private void OpenControllerForm(FormControllerBase newController)
        {
            if (_backEffect)
                this.BackToForm(newController.Form);
            else
                this.OpenForm(newController.Form);
        }

        private void DisposeController(ref FormControllerBase currentController)
        {
            if (currentController is IKinectFunctionsController)
                ((IKinectFunctionsController)currentController).DisposeKinectFunctions(_kinectSensorManager);

            if (currentController is ISpeechBarController)
            {
                ISpeechBarController controller = currentController as ISpeechBarController;

                if (this.SpeechBarStatusChanged != null)
                    this.SpeechBarStatusChanged -= controller.OnSpeechBarStatusChanged;

                if (this.SpeechRecognized != null)
                    this.SpeechRecognized -= controller.SpeechRecognized;
            }
        }

        #endregion [Métodos Privados]

        #region [Implementação dos eventos]

        private void _kinectSensorManager_SpeechRecognized(string command, double confidenceLevel)
        {

            if (confidenceLevel > 0.5)
                switch (command)
                {
                    case "Kinect":
                        this.SpeechBar.Visibility = System.Windows.Visibility.Visible;
                        CallSpeechBarStatusChangedEvent(true);
                        break;
                    case "Cancel":
                        if (this.SpeechBar.CommandCancel.Visibility == System.Windows.Visibility.Visible)
                        {
                            this.SpeechBar.Visibility = System.Windows.Visibility.Hidden;
                            CallSpeechBarStatusChangedEvent(false);
                        }
                        break;
                    default:
                        if (SpeechBar.Visibility == System.Windows.Visibility.Visible &&
                            CurrentController is ISpeechBarController &&
                            ((ISpeechBarController)CurrentController).AcceptableCommands.Contains(command))

                            if (this.SpeechRecognized != null)
                                this.SpeechRecognized(command, confidenceLevel);

                        break;
                }


        }

        #endregion [Implementação dos eventos]


    }
}
