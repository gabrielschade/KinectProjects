using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Center.Business.Classes
{
    public class DepthFrameParameters
    {
        #region [Propriedades]

        public DepthImagePixel[] BitsFrame { get; private set; }

        public int MaxDepth { get; private set; }

        public int MinDepth { get; private set; }

        public byte[] ColorPixels { get; private set; }

        #endregion [Propriedades]

        #region [Construtores]

        public DepthFrameParameters(DepthImageFrame depthFrame)
        {
            BitsFrame = new DepthImagePixel[depthFrame.PixelDataLength];
            depthFrame.CopyDepthImagePixelDataTo(BitsFrame);

            // Obtém a maior e a menor profundidade lida neste frame
            MinDepth = depthFrame.MinDepth;
            MaxDepth = depthFrame.MaxDepth;

            ColorPixels = new byte[depthFrame.PixelDataLength * sizeof(int)];
        }

        #endregion [Construtores]
    }
}
