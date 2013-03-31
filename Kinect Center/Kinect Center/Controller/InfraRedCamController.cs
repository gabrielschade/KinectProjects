using Kinect_Center.View;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Kinect_Center.Controller
{
    public class InfraRedCamController : InstantCamController
    {

        #region [Métodos Sobrescritos da Herança]

        public override void InitializeKinectFunctions(Business.Classes.KinectSensorManager kinectManager)
        {
            kinectManager.Kinect.ColorStream.Enable(ColorImageFormat.InfraredResolution640x480Fps30);
            kinectManager.Kinect.ColorFrameReady += Kinect_ColorFrameReady;
        }

        #endregion [Métodos Sobrescritos da Herança]

        #region [Métodos ligados a eventos]


        private void Kinect_ColorFrameReady(object sender, Microsoft.Kinect.ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame currentFrame = e.OpenColorImageFrame())
            {
                if (currentFrame != null)
                {
                    byte[] bitsFrame = new byte[currentFrame.PixelDataLength];
                    currentFrame.CopyPixelDataTo(bitsFrame);
                    BitmapSource source = BitmapImage.Create(currentFrame.Width, currentFrame.Height, 96, 96, System.Windows.Media.PixelFormats.Gray16, null, bitsFrame, currentFrame.Width * currentFrame.BytesPerPixel);
                    this.GetView<Camera>().UpdateCanvas(source);
                }
            }
        }

        #endregion [Métodos ligados a eventos]

    }
}
