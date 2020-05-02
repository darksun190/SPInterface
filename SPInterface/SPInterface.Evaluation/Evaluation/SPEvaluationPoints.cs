using SPInterface.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SPInterface.Core;

namespace SPInterface.Evaluation
{
    /// <summary>
    /// points set for outlier or filtering
    /// </summary>
    internal class SPEvaluationPoints : List<SPEvaluationPoint>
    {
        public SPEvaluationPoints getOutlierPoints(int factor)
        {
            return getOutlierPoints(factor, factor);
        }
        public SPEvaluationPoints getOutlierPoints(int plusfactor, int minusfactor)
        {
            Outlier<SPEvaluationPoint> outlier = new Outlier<SPEvaluationPoint>(this, n => n.Deviation, plusfactor, minusfactor);
            return (from u in outlier.OutlierResult
                    where u.Masked == false
                    select u).ToList() as SPEvaluationPoints;
        }

        internal List<FeatureMeasurePoint> toFeatureMeasurePoints()
        {
            return this.Select(n => n.rawPoint).ToList();
        }
        internal List<double> toDeviations()
        {
            return this.Select(n => n.Deviation).ToList();
        }
        static public SPEvaluationPoints createFromLine(SPLine line)
        {
            throw new NotImplementedException();
        }
        static public SPEvaluationPoints createFromCircle(SPCircle circle)
        {
            throw new NotImplementedException();
        }
    }
}
