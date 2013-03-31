using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyComponents.Controls
{
    /// <summary>
    /// Interaction logic for SettingsForm.xaml
    /// </summary>
    public partial class SettingsForm : ContentForm
    {
        #region [Attributes]

        private UIElement _content;

        #endregion end of [Attributes]

        #region [Constructors]

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SettingsForm()
        {
            InitializeComponent();
        }

        #endregion end of [Constructors]

        #region [Methods related to display controls events]

        private void Close_Click( object sender, RoutedEventArgs e )
        {
            CloseSettings();
        }

        #endregion end of [Methods related to display controls events]

        #region [Public Methods]

        /// <summary>
        /// This method close the settings form
        /// </summary>
        public void CloseSettings()
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
            if ( _content != null )
            {
                _content = null;
            }

        }

        /// <summary>
        /// This method open the settings form with content that was sent by parameter
        /// </summary>
        /// <param name="content">Content</param>
        public void ShowSettings( SettingsContentForm content )
        {
            _content = content;
            this.pnlComponents.Children.Add( _content );
            this.Visibility = System.Windows.Visibility.Visible;
            Panel.SetZIndex( this, 100 );
        }

        #endregion end of [Public Methods]

        #region [ISpeechBarObserver Members]

        /// <summary>
        /// This method are invoked when the user says "Kinect"
        /// </summary>
        /// <param name="sender">The main form of application</param>
        /// <param name="e">Event args</param>
        public void SpeechBarActivate( object sender, EventArgs e )
        {
            txtCloseButton.FontFamily = new FontFamily( "Segoe UI Semibold" );
        }

        /// <summary>
        /// This method are invoked when the user says "Cancel"
        /// </summary>
        /// <param name="sender">The main form of application</param>
        /// <param name="e">Event args</param>
        public void SpeechBarDeactivate( object sender, EventArgs e )
        {
            txtCloseButton.FontFamily = new FontFamily( "Segoe UI" );
        }

        #endregion  end of [ISpeechBarObserver Members]
    }
}
