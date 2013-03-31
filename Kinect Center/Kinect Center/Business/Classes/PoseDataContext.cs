using Microsoft.Kinect;
using MyKinectLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Center.Business.Classes
{
    public class PoseDataContext
    {
        #region [Propriedades]

        public Pose CurrentPose { get; set; }

        public Skeleton CurrentUserSkeleton
        {
            get { return CurrentPose.UserSkeleton; }
            set { CurrentPose.UserSkeleton = value; }
        }

        #endregion [Propriedades]

        #region [Construtores]

        public PoseDataContext()
        {
            this.CurrentPose = new Pose();
        }

        #endregion [Construtores]
    }
}
