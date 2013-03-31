using Kinect_Center.Business.Classes;
using Kinect_Center.View;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Kinect_Center.Controller
{
    public class DepthCamController : InstantCamController
    {
        #region [Atributos]

        WriteableBitmap colorBitmap;
        Dispatcher appThread;

        #endregion [Atributos]


        #region [Métodos Sobrescritos da Herança]

        public override void InitializeKinectFunctions(Business.Classes.KinectSensorManager kinectManager)
        {
            if (!kinectManager.Kinect.DepthStream.IsEnabled)
                kinectManager.Kinect.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
            kinectManager.Kinect.DepthFrameReady += Kinect_DepthFrameReady;

            //Obtem a thread da aplicação
            this.appThread = Dispatcher.CurrentDispatcher;

            this.colorBitmap = new WriteableBitmap(kinectManager.Kinect.DepthStream.FrameWidth, kinectManager.Kinect.DepthStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);
            this.GetView<Camera>().GetImageMethod += ConvertWriteableBitmapToBitmapImage;
        }

        public override void DisposeKinectFunctions(Business.Classes.KinectSensorManager kinectManager)
        {
            kinectManager.Kinect.DepthFrameReady -= Kinect_DepthFrameReady;
            this.appThread = null;
        }

        #endregion [Métodos Sobrescritos da Herança]

        #region [Métodos protegidos]

        private void Kinect_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            using (DepthImageFrame currentFrame = e.OpenDepthImageFrame())
            {
                if (currentFrame != null)
                {
                    DepthFrameParameters parameters = new DepthFrameParameters(currentFrame);
                    Action<object> actionMethod = new Action<object>(ConvertDepthToColor);
                    Task task = new Task(actionMethod, parameters);
                    task.Start();
                }
            }
        }

        #endregion [Métodos protegidos]

        #region [Métodos privados]

        public BitmapImage ConvertWriteableBitmapToBitmapImage(ImageSource source)
        {
            WriteableBitmap bitmap = source as WriteableBitmap;
            BitmapImage bmImage = new BitmapImage();
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(stream);
                bmImage.BeginInit();
                bmImage.CacheOption = BitmapCacheOption.OnLoad;
                bmImage.StreamSource = stream;
                bmImage.EndInit();
                bmImage.Freeze();
            }
            return bmImage;
        }

        private void ConvertDepthToColor(object parameters)
        {
            if (parameters is DepthFrameParameters)
                ConvertDepthToColor(parameters as DepthFrameParameters);
            else
                throw new InvalidCastException("Current frame is not a DepthImageFrame");
        }

        private void ConvertDepthToColor(DepthFrameParameters depthParameters)
        {
            // Vai ser utilizado para converter o depth em rgb
            int colorPixelIndex = 0;
            for (int index = 0; index < depthParameters.BitsFrame.Length; ++index)
            {
                //Obtem a profundidade do pixel
                short depth = depthParameters.BitsFrame[index].Depth;
                //obtem a intensidade do pixel, caso esteja fora do intervalo é traduzido para 0 (pixel preto)
                byte intensity = (byte)(depth >= depthParameters.MinDepth && depth <= depthParameters.MaxDepth ? depth : 0);

                //escala cinza do rgb de acordo com a profundidade do pixel
                // byte do Blue do RGB
                depthParameters.ColorPixels[colorPixelIndex++] = intensity;

                //Byte do Green do RGB
                depthParameters.ColorPixels[colorPixelIndex++] = intensity;

                //Byte do Red do RGB                    
                depthParameters.ColorPixels[colorPixelIndex++] = intensity;

                ++colorPixelIndex;
            }

            if (appThread != null)
                appThread.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                    (System.Threading.ThreadStart)delegate()
                    {
                        this.colorBitmap.WritePixels(
                         new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight),
                         depthParameters.ColorPixels,
                         this.colorBitmap.PixelWidth * sizeof(int),
                         0);
                        this.GetView<Camera>().UpdateCanvas(colorBitmap);
                    });

        }
        #endregion [Métodos privados]

    }
}
