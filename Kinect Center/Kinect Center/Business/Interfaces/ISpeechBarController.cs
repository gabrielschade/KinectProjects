using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Center.Business.Interfaces
{
    public interface ISpeechBarController
    {
        /// <summary>
        /// Comandos aceitos pelo controller
        /// </summary>
        List<string> AcceptableCommands { get; }

        void SpeechRecognized(string command, double confidenceLevel);
        void OnSpeechBarStatusChanged(bool isActivated);
    }
}
