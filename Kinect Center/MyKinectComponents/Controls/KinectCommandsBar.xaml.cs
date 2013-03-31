using Microsoft.Kinect.Toolkit.Controls;
using MyComponents.Controls;
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

namespace MyKinectComponents.Controls
{
    /// <summary>
    /// Interaction logic for SpeechBar.xaml
    /// </summary>
    public partial class KinectCommandsBar : ContentForm
    {
        #region [Atributos]

        private List<KinectCircleButton> _buttons;

        #endregion [Atributos]

        #region [Construtores]

        /// <summary>
        /// DefaultConstructor
        /// </summary>
        public KinectCommandsBar()
        {
            InitializeComponent();
            _buttons = new List<KinectCircleButton>();
            _buttons.Add(btnBack);
        }

        #endregion end of [Construtores]

        #region [Propriedades]

        public bool IsActive { get { return this.Visibility == System.Windows.Visibility.Visible; } }

        public KinectCircleButton CommandBack { get { return btnBack; } }

        #endregion [Propriedades]

        #region [Public Methods]

        /// <summary>
        /// This method shows this component
        /// </summary>
        public void ShowCommandsBar()
        {
            if (this.Visibility != Visibility.Visible)
            {
                this.Visibility = System.Windows.Visibility.Visible;
            }
        }

        /// <summary>
        /// This method hides this component
        /// </summary>
        public void HideCommandsBar()
        {
            if (this.Visibility == Visibility.Visible)
            {

                this.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        public void AddCommand(string command, RoutedEventHandler clickEvent, ImageSource icon)
        {
            if (_buttons.Any(button => button.Label == command))
                throw new InvalidOperationException("This command already exists");

            KinectCircleButton newCommand = new KinectCircleButton();
            newCommand.Label = command;
            newCommand.Click += clickEvent;
            newCommand.Foreground = Brushes.White;
            newCommand.Height = 250;
            newCommand.Width = 200;
            newCommand.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            newCommand.Margin = new Thickness(0, -15, 0, 0);
            newCommand.FontFamily = btnBack.FontFamily;

            Image imageIcon = new Image();
            imageIcon.Source = icon;
            imageIcon.Height = 50;
            imageIcon.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            newCommand.Content = imageIcon;
            pnlCommands.Children.Add(newCommand);
            _buttons.Add(newCommand);
        }

        public void RemoveCommand(string command)
        {
            if (_buttons.Any(button => button.Label == command))
            {
                KinectCircleButton buttonToRemove = _buttons.First(button => button.Label == command);
                pnlCommands.Children.Remove(buttonToRemove);
            }
            else
                throw new InvalidOperationException("This command not exists");
        }

        public void UpdateFont(FontFamily font)
        {
            foreach (KinectCircleButton button in _buttons)
            {
                button.FontFamily = font;
            }
        }

        #endregion end of [Public Methods]
    }
}
