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
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace MyComponents.NavigationPanel
{
    /// <summary>
    /// This component manages the navigability between the users controls of Input Register App
    /// </summary>
	public partial class NavigationPanel : MahApps.Metro.Controls.MetroContentControl
    {
        #region [Attributes]

        /// <summary>
        /// Attribute to manage the pages
        /// </summary>
        private Stack<ContentControl> pages = new Stack<ContentControl>();

        #endregion end of [Attributes]

        #region [Properties]

        /// <summary>
        /// Current Page
        /// </summary>
        public ContentControl CurrentPage { get; set; }

        /// <summary>
        /// Dependency Property of Transtion Type
        /// </summary>
		public static readonly DependencyProperty TransitionTypeProperty = DependencyProperty.Register("TransitionType",
			typeof(NavigationType),
            typeof( NavigationPanel ), new PropertyMetadata( NavigationType.SlideAndFade ) );

        /// <summary>
        /// Transition Type
        /// </summary>
        public NavigationType TransitionType
		{
			get
			{
                return ( NavigationType ) GetValue( TransitionTypeProperty );
			}
			set 
			{
				SetValue(TransitionTypeProperty, value);
			}
		}

        #endregion end of [Properties]

        #region [Constructors]

        /// <summary>
        /// Default Constructor
        /// </summary>
        public NavigationPanel()
		{
			InitializeComponent();
		}

        #endregion end of [Constructors]

        #region [Methods]

        /// <summary>
        /// This method shows the form that was sent by parameter
        /// </summary>
        /// <param name="form">Form to show</param>
        public void ShowForm( ContentControl form )
		{			
			pages.Push(form);
            this.TransitionType = NavigationType.SlideAndFade;
			Task.Factory.StartNew(() => ShowNewForm());
		}

        /// <summary>
        /// This method shows the form that was sent by parameter with slide back effect
        /// </summary>
        /// <param name="form">Form to show</param>
        public void BackToForm( ContentControl form )
        {
            pages.Push( form );
            this.TransitionType = NavigationType.SlideAndFadeBack;
            Task.Factory.StartNew( () => ShowNewForm() );
        }

        /// <summary>
        /// This method shows a new form
        /// </summary>
		private void ShowNewForm()
		{
			Dispatcher.Invoke((Action)delegate 
				{
					if (contentPresenter.Content != null)
					{
                        ContentControl oldPage = contentPresenter.Content as ContentControl;

						if (oldPage != null)
						{
							oldPage.Loaded -= newForm_Loaded;

							UnloadForm(oldPage);
						}
					}
					else
					{
						ShowNextForm();
					}
					
				});
		}

        /// <summary>
        /// This method shows a next form
        /// </summary>
		private void ShowNextForm()
		{
			ContentControl newPage = pages.Pop();

			newPage.Loaded += newForm_Loaded;

			contentPresenter.Content = newPage;
		}

        /// <summary>
        /// This method hide a form
        /// </summary>
        /// <param name="form">Form to hidden</param>
        private void UnloadForm( ContentControl form )
		{
			Storyboard hidePage = (Resources[string.Format("{0}Out", TransitionType.ToString())] as Storyboard).Clone();

			hidePage.Completed += hideForm_Completed;

			hidePage.Begin(contentPresenter);
		}

        #endregion end of [Methods]

        #region [Methods linked to events]

        private void newForm_Loaded( object sender, RoutedEventArgs e )
		{
			Storyboard showNewPage = Resources[string.Format("{0}In", TransitionType.ToString())] as Storyboard;

			showNewPage.Begin(contentPresenter);

			CurrentPage = sender as ContentControl;
		}

        private void hideForm_Completed( object sender, EventArgs e )
		{
			contentPresenter.Content = null;

			ShowNextForm();
        }

        #endregion end of [Methods linked to events]
    }
}
