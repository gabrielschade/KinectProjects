using Kinect_Center.Controller;
using Microsoft.Kinect.Toolkit.Controls;
using MyComponents.Controls;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Kinect_Center.View
{
    /// <summary>
    /// Interaction logic for InstantCam.xaml
    /// </summary>
    public partial class Camera : ContentForm
    {
        #region [Constantes]

        private const int PICTURE_HEIGHT = 200;
        private const int PICTURE_WEIGHT = 240;
        private const int PANEL_EXTRA_HEIGTH = 50;

        #endregion [Constantes]

        #region [Propriedades]

        public DispatcherTimer Timer { get; private set; }

        public GetImage GetImageMethod { get; set; }

        #endregion [Propriedades]

        #region [Construtores]

        public Camera()
        {
            InitializeComponent();
            InitializeKinectRegion();
            Timer = new DispatcherTimer();
        }

        #endregion [Construtores]

        #region [Delegates]

        public delegate BitmapImage GetImage(ImageSource source);

        #endregion [Delegates]

        #region [Métodos Públicos]

        public void UpdateCanvas(BitmapSource source)
        {
            cameraImage.Source = source;
        }

        public void SavePicture()
        {
            ImageSource source;

            if (GetImageMethod != null)
                source = GetImageMethod(cameraImage.Source);
            else
                source = cameraImage.Source;

            KinectTileButton pictureTile = new KinectTileButton();
            MyComponents.VisibilityAnimation.VisibilityAnimation.SetAnimationType(pictureTile, MyComponents.VisibilityAnimation.VisibilityAnimation.AnimationType.Fade);
            pictureTile.Foreground = Brushes.White;
            pictureTile.Height = PICTURE_HEIGHT;
            pictureTile.Width = PICTURE_WEIGHT;

            Grid gridPicture = new Grid();
            gridPicture.Width = pictureTile.Width;
            gridPicture.Height = pictureTile.Height;

            gridPicture.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            gridPicture.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50, GridUnitType.Pixel) });

            Image imgPicture = new Image();

            imgPicture.Source = source;
            imgPicture.Stretch = Stretch.UniformToFill;

            Label lbPicture = new Label();
            lbPicture.Content = string.Format("Picture {0}", pnlPictures.Children.Count + 1);
            lbPicture.FontFamily = new FontFamily("Segoe UI Light");
            lbPicture.FontSize = 32;
            lbPicture.Foreground = Brushes.White;
            lbPicture.Background = new SolidColorBrush(Color.FromRgb(78, 134, 255));


            gridPicture.Children.Add(lbPicture);
            gridPicture.Children.Add(imgPicture);

            Grid.SetRow(imgPicture, 0);
            Grid.SetRow(lbPicture, 1);

            pictureTile.Content = gridPicture;
            pictureTile.Tag = imgPicture;
            pictureTile.Click += Picture_Click;

            pnlPictures.Children.Insert(0, pictureTile);
            pnlPictures.Height = pnlPictures.Children.Count * PICTURE_HEIGHT + PANEL_EXTRA_HEIGTH;
        }



        #endregion

        #region [Métodos privados]

        private void InitializeKinectRegion()
        {
            Binding regionSensorBinding = new Binding("Kinect") { Source = FrontController.Instance.KinectSensorManager };
            BindingOperations.SetBinding(this.kinectRegion, KinectRegion.KinectSensorProperty, regionSensorBinding);

            this.commandsBar.CommandBack.Click += CommandBack_Click;
        }

        #endregion [Métodos privados]

        private void ContentForm_Loaded(object sender, RoutedEventArgs e)
        {
            FrontController.Instance.ShowSpeechBar();
            this.pnlPictures.Children.Clear();
        }

        private void Picture_Click(object sender, RoutedEventArgs e)
        {
            KinectTileButton button = sender as KinectTileButton;
            Image image = button.Tag as Image;
            ImageVisualizer imageVisualizer = new ImageVisualizer(image.Source);
            imageVisualizer.ShowDialog();
        }

        private void CommandBack_Click(object sender, RoutedEventArgs e)
        {
            FrontController.Instance.BackToHome();
        }

        internal void AddTakeaPictureCommand(RoutedEventHandler btnTakeAPicture_Click)
        {
            this.commandsBar.AddCommand("Picture", btnTakeAPicture_Click, new BitmapImage(new Uri("/Kinect Center;component/Resources/Icons/cam.png", UriKind.Relative)));
        }

        internal void UpdateFont(System.Windows.Media.FontFamily fontFamily)
        {
            commandsBar.UpdateFont(fontFamily);
        }
    }
}
