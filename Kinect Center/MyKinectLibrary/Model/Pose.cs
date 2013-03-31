using Microsoft.Kinect;
using MyKinectLibrary.Enums;
using System.Collections.Generic;
using System.Diagnostics;

namespace MyKinectLibrary.Model
{
    /// <summary>
    /// This classe is a Pose model data
    /// </summary>
    [DebuggerDisplay( "Name:{Name} CurrentFrame:{CurrentFrame} Status:{Status} WaitingTime:{WaitingTime}" )]
    public class Pose
    {
        #region [Properties]

        /// <summary>
        /// Get or set the value of current frame of pose in runtime
        /// </summary>
        /// <remarks>
        /// This property is used only in runtime.
        /// This property is used to manage the time to activate the pose.
        /// </remarks>
        public int CurrentFrame
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the value of Pose state: Not started, in progress or accepted
        /// </summary>
        /// <remarks>
        /// This property is used only in runtime.
        /// </remarks>
        public PoseStatus Status
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the value of Pose name
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the value of a Waiting time to change status of this pose from "in_progress" to "accepted"
        /// </summary>
        public int WaitingTime
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set a collection of subPoses that compose this pose
        /// </summary>
        public List<SubPose> SubPoses
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the full skeleton of this pose
        /// </summary>
        public Skeleton UserSkeleton
        { get; set; }

        #endregion end of [Properties]

        #region [Constructor]

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Pose()
        {
            this.Status = PoseStatus.not_started;
            this.SubPoses = new List<SubPose>();
        }

        #endregion end of [Constructor]
    }
}
