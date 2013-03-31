using MyKinectLibrary.Enums;
using MyKinectLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKinectLibrary.Delegates
{
    public delegate void PoseStatusChanged(Pose pose, PoseEventStatus status);
}
