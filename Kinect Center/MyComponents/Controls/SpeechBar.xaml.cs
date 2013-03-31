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

namespace MyComponents.Controls
{
    /// <summary>
    /// Interaction logic for SpeechBar.xaml
    /// </summary>
    public partial class SpeechBar : ContentForm
    {
        #region [Construtores]

        /// <summary>
        /// DefaultConstructor
        /// </summary>
        public SpeechBar()
        {
            InitializeComponent();
        }

        #endregion end of [Construtores]

        #region [Propriedades]

        public bool IsActive { get { return this.Visibility == System.Windows.Visibility.Visible; } }

        public Label CommandCancel { get { return lblCommandCancel; } }

        #endregion [Propriedades]

        #region [Public Methods]

        /// <summary>
        /// This method shows this component
        /// </summary>
        public void ShowSpeechBar()
        {
            if (this.Visibility != Visibility.Visible)
            {
                if (SpeechBarActive != null)
                    SpeechBarActive(this, new EventArgs());

                this.Visibility = System.Windows.Visibility.Visible;
                this.lblSpeechBar.Visibility = System.Windows.Visibility.Visible;
            }
        }

        /// <summary>
        /// This method hides this component
        /// </summary>
        public void HideSpeechBar()
        {
            if (this.Visibility == Visibility.Visible)
            {
                if (SpeechBarDeactive != null)
                    SpeechBarDeactive(this, new EventArgs());

                this.Visibility = System.Windows.Visibility.Hidden;
                this.lblSpeechBar.Visibility = System.Windows.Visibility.Visible;
            }
        }

        public void AddCommand(string command)
        {
            if (command.Equals("Cancel"))
                this.lblCommandCancel.Visibility = System.Windows.Visibility.Visible;
            else
            {
                UIElement[] elements = new UIElement[this.pnlCommands.Children.Count];
                pnlCommands.Children.CopyTo(elements, 0);

                if (elements.Any(element => element is Label && ((Label)element).Content == command))
                {
                    throw new InvalidOperationException("O comando já foi adicionado para a SpeechBar");
                }

                Label newCommand = new Label();
                newCommand.Content = command;
                newCommand.FontFamily = new FontFamily("Segoe UI Semibold");
                newCommand.FontSize = 28;
                newCommand.Foreground = Brushes.White;
                newCommand.Margin = new Thickness(10, 25, 10, 0);

                this.pnlCommands.Children.Add(newCommand);
            }
        }

        public void RemoveCommand(string command)
        {
            if (command.Equals("Cancel"))
                this.lblCommandCancel.Visibility = Visibility.Collapsed;
            else
            {
                UIElement[] elements = new UIElement[this.pnlCommands.Children.Count];
                pnlCommands.Children.CopyTo(elements, 0);
                var commandLabel = elements.Where(element => element is Label && ((Label)element).Content.Equals(command));
                if (commandLabel.Count() > 0)
                {
                    pnlCommands.Children.Remove(commandLabel.First());
                }
                else
                    throw new InvalidOperationException("O comando não existe na SpeechBar");
            }
        }

        #endregion end of [Public Methods]

        #region [Events]

        /// <summary>
        /// This event occurs when the user actives the speech bar
        /// </summary>
        public event EventHandler SpeechBarActive;

        /// <summary>
        /// This event occurs when the user deactivate the speech bar
        /// </summary>
        public event EventHandler SpeechBarDeactive;

        #endregion end of [Events]



    }
}
