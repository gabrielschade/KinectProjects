using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Center.Business.Classes
{
    public class BeamManager : INotifyPropertyChanged
    {
        readonly KinectAudioSource audioSource;

        public BeamManager(KinectAudioSource source)
        {
            audioSource = source;
            audioSource.BeamAngleChanged += audioSource_BeamAngleChanged;
        }

        void audioSource_BeamAngleChanged(object sender, BeamAngleChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("BeamAngle"));
            }
        }

        public double BeamAngle
        {
            get
            {
                return Math.Max(0, Math.Min(1, (audioSource.BeamAngle - KinectAudioSource.MinBeamAngle) / (KinectAudioSource.MaxBeamAngle - KinectAudioSource.MinBeamAngle)));
            }
        }

        public void Dispose()
        {
            audioSource.BeamAngleChanged -= audioSource_BeamAngleChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
