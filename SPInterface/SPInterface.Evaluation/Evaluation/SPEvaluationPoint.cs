using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPInterface.Core;

namespace SPInterface.Evaluation
{
    /// <summary>
    /// only use for outlier & filtering
    /// </summary>
    internal class SPEvaluationPoint
    {
        public FeatureMeasurePoint rawPoint { get; set; }
        /// <summary>
        /// the X value, could be distance to 0 point or radian value
        /// </summary>
        public double SampleIndex { get; set; }
        /// <summary>
        /// devaitions used for outlier or filtering
        /// </summary>
        public double Deviation { get; set; }
        /// <summary>
        /// signal if this point was only for padding
        /// </summary>
        public bool Masked { get; set; }
    }
}
