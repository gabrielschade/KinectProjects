using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKinectLibrary.Util
{
    public static class ExtensionMethodsManager
    {
        public static string ToText(this JointType enumeration)
        {
            string jointName;
            switch (enumeration)
            {
                case JointType.HipCenter:
                    jointName = "Hip Center";
                    break;
                case JointType.Spine:
                    jointName = "Spine";
                    break;
                case JointType.ShoulderCenter:
                    jointName = "Shoulder Center";
                    break;
                case JointType.Head:
                    jointName = "Head";
                    break;
                case JointType.ShoulderLeft:
                    jointName = "Left Shoulder";
                    break;
                case JointType.ElbowLeft:
                    jointName = "Left Elbow";
                    break;
                case JointType.WristLeft:
                    jointName = "Left Wrist";
                    break;
                case JointType.HandLeft:
                    jointName = "Left Hand";
                    break;
                case JointType.ShoulderRight:
                    jointName = "Right Shoulder";
                    break;
                case JointType.ElbowRight:
                    jointName = "Right Elbow";
                    break;
                case JointType.WristRight:
                    jointName = "Right Wrist";
                    break;
                case JointType.HandRight:
                    jointName = "Right Hand";
                    break;
                case JointType.HipLeft:
                    jointName = "Left Hip";
                    break;
                case JointType.KneeLeft:
                    jointName = "Left Knee";
                    break;
                case JointType.AnkleLeft:
                    jointName = "Left Ankle";
                    break;
                case JointType.FootLeft:
                    jointName = "Left Foot";
                    break;
                case JointType.HipRight:
                    jointName = "Right Hip";
                    break;
                case JointType.KneeRight:
                    jointName = "Right Knee";
                    break;
                case JointType.AnkleRight:
                    jointName = "Right Ankle";
                    break;
                case JointType.FootRight:
                    jointName = "Right Foot";
                    break;
                default:
                    jointName = "Unknown Joint";
                    break;
            }

            return jointName;
        }
    }
}
