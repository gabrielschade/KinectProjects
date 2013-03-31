using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyKinectLibrary.Model.Auxiliary
{
    /// <summary>
    /// This class manages the results of all angle calculation
    /// </summary>
    public class AngleCalculation
    {
        #region [Properties]

        /// <summary>
        /// Get or set the value of Scalar Product calculation result
        /// </summary>
        internal double ScalarProductResult 
        { get; set; }

        /// <summary>
        /// Get or set the value of Law of Cosines calculation result in all plans
        /// </summary>
        internal LawOfCosinesResult LawOfCosinesResult
        { get; set; } 

        #endregion end of [Properties]

        #region [Constructors]

        /// <summary>
        /// Default constructor
        /// </summary>
        public AngleCalculation()
        { }

        /// <summary>
        /// Constructor that initializes the result properties
        /// </summary>
        /// <param name="scalarProductResult">The result of Scalar Product calculation</param>
        /// <param name="lawOfCosinesResult">The result of Law of Cosines calculation in all plans</param>
        internal AngleCalculation( double scalarProductResult, LawOfCosinesResult lawOfCosinesResult )
        {
            this.LawOfCosinesResult = lawOfCosinesResult;
            this.ScalarProductResult = scalarProductResult;
        }

        #endregion end of [Constructors]
    }
}
