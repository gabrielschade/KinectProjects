using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Center.Business.Delegates
{
    public delegate void SpeechRecognized(string command, double confidenceLevel);
    public delegate void SpeechBarStatusChanged(bool isActivated);
    public delegate void GameEnd();
}
