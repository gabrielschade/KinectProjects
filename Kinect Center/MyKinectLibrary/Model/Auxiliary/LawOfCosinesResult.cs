using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyKinectLibrary.Model.Auxiliary
{
    /// <summary>
    /// This class contains the values of Law of Cosines result calculated in all plans
    /// </summary>
    internal class LawOfCosinesResult
    {

        #region [Properties]

        /// <summary>
        /// Get or Set the value of result in plan XY
        /// </summary>
        internal double PlanXY { get; set; }
        /// <summary>
        /// Get or Set the value of result in plan XZ
        /// </summary>
        internal double PlanXZ { get; set; }

        /// <summary>
        /// Get or Set the value of result in plan YZ
        /// </summary>
        internal double PlanYZ { get; set; }

        #endregion end of [Properties]

        #region [Constructors]

        /// <summary>
        /// Default constructor
        /// </summary>
        internal LawOfCosinesResult()
        { }

        /// <summary>
        /// Constructor that initializes the result properties per plan
        /// </summary>
        /// <param name="planXY">Result in XY plan</param>
        /// <param name="planXZ">Result in XZ plan</param>
        /// <param name="planYZ">Result in YZ plan</param>
        internal LawOfCosinesResult( double planXY, double planXZ, double planYZ )
        {
            this.PlanXY = planXY;
            this.PlanXZ = planXZ;
            this.PlanYZ = planYZ;
        }

        #endregion end of [Constructors]

    }
}
