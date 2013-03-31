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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyComponents.Controls
{
    /// <summary>
    /// Interaction logic for HandTracker.xaml
    /// </summary>
    public partial class HandTracker : UserControl
    {
        public event RoutedEventHandler Click;

		public Storyboard uxSBHold, uxSBPing;
		private bool _isHeld;

        public HandTracker()
        {
            InitializeComponent();

            uxSBHold = FindResource("uxSBHold") as Storyboard;
            uxSBPing = FindResource("uxSBPing") as Storyboard;

            if (uxSBHold != null)
                uxSBHold.Completed += uxSBHold_Completed;

            if( uxSBPing != null)
                uxSBPing.Completed += uxSBPing_Completed;
        }

        public int TimeInterval
        {
            get { return _timeInterval; }
            set
            {
                _timeInterval = value;

                if (uxSBHold.Children.Count > 0 && TimeInterval > 0)
                {
                    var arcAnimation = uxSBHold.Children[0] as DoubleAnimationUsingKeyFrames;
                    if (arcAnimation == null)
                        return;

                    arcAnimation.KeyFrames.Clear();

                    var start = new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.Zero));
                    var end = new LinearDoubleKeyFrame(360, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, TimeInterval)));

                    arcAnimation.KeyFrames.Add(start);
                    arcAnimation.KeyFrames.Add(end);
                }
            }
        }
        private int _timeInterval;

		private double _imageSize;
		public double ImageSize
		{
			get { return _imageSize; }
            set
            {
                _imageSize = value;
                Height = Width = ImageSize;
            }
		}

		private string _imageSource;
		public string ImageSource
		{
			get { return _imageSource; }
			set
			{
				_imageSource = value;
				if(ImageSource!=null)
					uxButtonImage.Source = new BitmapImage(new Uri(ImageSource, UriKind.RelativeOrAbsolute));
			}
		}

		private string _activeImageSource;
		public string ActiveImageSource
		{
			get { return _activeImageSource; }
			set
			{
				_activeImageSource = value;
				if (ActiveImageSource != null)
                    uxButtonActiveImage.Source = new BitmapImage(new Uri(ActiveImageSource, UriKind.RelativeOrAbsolute));
			}
		}

		public bool IsTriggeredOnHover{ get; set; }

		private bool _isChecked;
		public bool IsChecked
		{
			get { return _isChecked; }
			set
			{
                if ((IsTriggeredOnHover && value && !_isChecked) || (!IsTriggeredOnHover && value != _isChecked))
                    uxSBPing.Begin();
                else
                {
                    _isChecked = value;

                    if ((IsChecked && IsTriggeredOnHover || !IsTriggeredOnHover) && Click != null)
                        Click(this, null);

                    VisualStateManager.GoToState(this, IsChecked ? "Checked" : "Normal", true);
                }
			}
		}

        void uxSBPing_Completed(object sender, EventArgs e)
        {
            _isChecked = true;

            if ((IsChecked && IsTriggeredOnHover || !IsTriggeredOnHover) && Click != null)
                Click(this, null);

            VisualStateManager.GoToState(this, IsChecked ? "Checked" : "Normal", true);
        }

		void uxSBHold_Completed(object sender, EventArgs e)
		{
            ToggleIsCheck();
		}

        public void Hovering()
        {
            ToggleButtons(true, uxSBHold.Begin);
        }

	    public void Release()
		{
            ToggleButtons(false, uxSBHold.Stop);
		}

        private void ToggleButtons(bool wantedState, Action action)
        {
            if (_isHeld != wantedState)
            {
                _isHeld = wantedState;

                if (IsTriggeredOnHover)
                    ToggleIsCheck();
                else
                    action();
            }
        }

        private void ToggleIsCheck()
        {
            IsChecked = !IsChecked;
        }
    }
}
