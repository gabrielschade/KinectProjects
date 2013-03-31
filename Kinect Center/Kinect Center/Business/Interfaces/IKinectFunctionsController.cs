using Kinect_Center.Business.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Center.Business.Interfaces
{
    public interface IKinectFunctionsController
    {
        void InitializeKinectFunctions(KinectSensorManager kinectManager);
        void DisposeKinectFunctions(KinectSensorManager kinectManager);
    }
}
