using MyKinectLibrary.Enums;
using MyKinectLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKinectLibrary.Delegates
{
    
    public delegate void PoseStatusChangedEventHandler(Pose pose, PoseEventStatus status);

    public delegate void PoseRecognizedEventHandler(Pose pose);

    public delegate void PoseInProgressEventHandler(Pose pose);

    public delegate void PoseInterruptedEventHandler(Pose pose);
}
