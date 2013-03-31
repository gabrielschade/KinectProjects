using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Kinect;
using MyKinectLibrary.Model.Auxiliary;
using MyKinectLibrary.PoseTracking;

namespace MyKinectLibrary.Model
{
    /// <summary>
    /// This classe is a subPose model data
    /// </summary>
    [DebuggerDisplay("Angle:{Angle} AngleMarginError:{AngleMarginError}")]
    public class SubPose
    {
        #region [Properties]

        /// <summary>
        /// Get or set the value for the SubPose angle between the three joints
        /// </summary>
        public AngleCalculation Angle
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the value for the margin error of property Angle
        /// </summary>
        public double AngleMarginError
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the value for the center joint
        /// </summary>
        public Joint CenterJoint
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the value for the auxiliary joint 1
        /// </summary>
        public Joint AuxiliaryJoint1
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the value for the center joint 2
        /// </summary>
        public Joint AuxiliaryJoint2
        {
            get;
            set;
        }

        #endregion end of [Properties]

        #region [Constructors]

        /// <summary>
        /// Default constructor
        /// </summary>
        public SubPose()
        { }

        /// <summary>
        /// Constructor that initializes the joints properties
        /// </summary>
        /// <param name="centerJoint">Center Joint</param>
        /// <param name="auxiliaryJoint1">Auxiliary Joint 1</param>
        /// <param name="auxiliaryJoint2">Auxiliary Joint 2</param>
        public SubPose(Joint centerJoint, Joint auxiliaryJoint1, Joint auxiliaryJoint2)
        {
            this.CenterJoint = centerJoint;
            this.AuxiliaryJoint1 = auxiliaryJoint1;
            this.AuxiliaryJoint2 = auxiliaryJoint2;
            this.AngleMarginError = 10;
            this.Angle = new AngleCalculation();
            this.Angle.LawOfCosinesResult = MathFunctionsManager.LawOfCosinesCalculation(this);
            this.Angle.ScalarProductResult = MathFunctionsManager.ScalarProductCalculation(this);
        }

        #endregion end of [Constructors]

    }
}
