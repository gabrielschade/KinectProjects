using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyKinectLibrary.Model.Auxiliary
{
    /// <summary>
    /// This class represents a vector in 3D coordinates system
    /// </summary>
    internal class Vector3D
    {
        #region [Properties]

        /// <summary>
        /// This property represents a value of X plan
        /// </summary>
        internal double X { get; set; }

        /// <summary>
        /// This property represents a value of Y plan
        /// </summary>
        internal double Y { get; set; }

        /// <summary>
        /// This property represents a value of Z plan
        /// </summary>
        internal double Z { get; set; }

        #endregion end of [Properties]
    }
}
