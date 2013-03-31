using MyComponents.Enums;
using MyLibrary.Util;
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MyComponents.Forms
{
    /// <summary>
    /// Interaction logic for DialogForm.xaml
    /// </summary>
    public partial class DialogForm : MetroForm
    {
        #region [Attributes]

        DispatcherTimer _timerToAutoClose;

        #endregion end of [Attributes]

        #region [Properties]

        /// <summary>
        /// Indicates the time (in milliseconds) to close this form when the <see cref="P:InputRegisterApp.Forms.OthersForms.DialogForm.AutoClose"/> is true
        /// </summary>
        public int? TimeToAutoClose { get; set; }

        #endregion end of [Properties]

        #region [Constructors]

        /// <summary>
        /// Default Constructor that initializes this form
        /// </summary>
        /// <param name="type">Type of this form</param>
        /// <param name="buttons">Buttons to show for user</param>
        /// <param name="title">Title of this form</param>
        /// <param name="message">Message to show for user</param>
        public DialogForm( DialogType type, DialogButtons buttons, string title, string message )
        {
            InitializeComponent();
            this.Title = title;
            this.txtMessage.Text = message;
            ConfigureButtons( buttons );
            ConfigureIcon( type );
        }

        #endregion end of [Constructors]

        #region [Private Methods]

        /// <summary>
        /// This method configure the icon of this window according the type of dialog
        /// </summary>
        /// <param name="type">Type of Dialog</param>
        private void ConfigureIcon( DialogType type )
        {
            BitmapImage IconImage = new BitmapImage();
            IconImage.BeginInit();
            switch ( type )
            {
                case DialogType.question:
                    IconImage.UriSource = new Uri("pack://application:,,,/MyComponents;component/Resources/Icons/Question.png");
                    break;
                case DialogType.info:
                    IconImage.UriSource = new Uri("pack://application:,,,/MyComponents;component/Resources/Icons/Info.png");
                    break;
                case DialogType.alert:
                    IconImage.UriSource = new Uri("pack://application:,,,/MyComponents;component/Resources/Icons/Alert.png");
                    break;
                case DialogType.error:
                    IconImage.UriSource = new Uri("pack://application:,,,/MyComponents;component/Resources/Icons/Error.png");
                    break;
            }
            IconImage.EndInit();
            this.Icon = IconImage;
        }

        /// <summary>
        /// This method configure the buttons of this window according the buttons that was sent by parameter
        /// </summary>
        /// <param name="buttons">Buttons Type</param>
        private void ConfigureButtons( DialogButtons buttons )
        {
            switch ( buttons )
            {
                case DialogButtons.ok:
                    btnCancelNo.Visibility = Visibility.Collapsed;
                    btnOkYes.Focus();
                    break;
                case DialogButtons.yes_no:
                    txtCancelNo.Text = "No";
                    txtOkYes.Text = "Yes";
                    btnCancelNo.Focus();
                    break;
                default:
                    btnCancelNo.Focus();
                    break;
            }
        }

        #endregion end of [Private Methods]

        #region [Methods linked to Events]

        /// <summary>
        /// Occurs when the btnCancelNo button was clicked
        /// <para>
        /// This method sets the DialogResult to false and closes this form
        /// </para>
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">Routed event args</param>
        private void btnCancelNo_Click( object sender, RoutedEventArgs e )
        {
            this.DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// Occurs when the btnOkYes button was clicked
        /// <para>
        /// This method sets the DialogResult to true and closes this form
        /// </para>
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">Routed event args</param>
        private void btnOkYes_Click( object sender, RoutedEventArgs e )
        {
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Occurs when the Visible property of this form is changed
        /// <para>
        /// Initialize the dispatcher timer to close this form if the <see cref="P:InputRegisterApp.Forms.OthersForms.DialogForm.AutoClose"/> property is true and <see cref="P:MyComponents.Forms.DialogForm.TimeToAutoClose"/> is more than zero.
        /// </para>
        /// </summary>
        /// <param name="sender">This form</param>
        /// <param name="e">Dependency Property Changed Event Args</param>
        private void MetroWindow_IsVisibleChanged( object sender, DependencyPropertyChangedEventArgs e )
        {
            if ( e.NewValue.To<bool>() )
            {
                if (TimeToAutoClose.HasValue && TimeToAutoClose.Value > 0)
                {
                    this.txtAutoClose.Visibility = System.Windows.Visibility.Visible;
                    _timerToAutoClose = new DispatcherTimer();
                    _timerToAutoClose.Interval = new TimeSpan( 0, 0, 0, 0, TimeToAutoClose.Value );
                    _timerToAutoClose.Tick += new EventHandler( CloseThisFormTickMethod );
                    _timerToAutoClose.Start();
                }
            }
        }

        /// <summary>
        /// This method occurs when the TimeToAutoClose is reached
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">Event Args</param>
        private void CloseThisFormTickMethod( object sender, EventArgs e )
        {
            _timerToAutoClose.Stop();
            _timerToAutoClose.Tick -= new EventHandler(CloseThisFormTickMethod);
            if (this.IsActive)
            {
                this.DialogResult = false;
                this.Close();
            }
        }

        #endregion end of [Methods linked to Events]


    }
}
