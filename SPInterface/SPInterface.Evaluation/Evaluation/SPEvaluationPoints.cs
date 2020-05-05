using SPInterface.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SPInterface.Core;
using System.Reflection;
using MathNet.Numerics;
using MathNet.Numerics.Statistics;
using SPInterface.Evaluation.Evaluation;

namespace SPInterface.Evaluation
{
    /// <summary>
    /// points set for outlier or filtering
    /// </summary>
    internal class SPEvaluationPoints : List<SPEvaluationPoint>
    {
        //public SPEvaluationPoints getOutlierPoints(int factor)
        //{
        //    return getOutlierPoints(factor, factor);
        //}
        public SPEvaluationPoints getOutlierPoints(int plusfactor, int minusfactor)
        {
            Outlier<SPEvaluationPoint> outlier = new Outlier<SPEvaluationPoint>(this, n => n.Deviation, plusfactor, minusfactor);
            return (from u in outlier.OutlierResult
                    where u.Masked == false
                    select u).ToList() as SPEvaluationPoints;
        }
        public SPEvaluationPoints getFilterPoints(int cutoffrate, FilterType filterType = FilterType.LowPass, FilterMethod filterMethod = FilterMethod.Gaussian)
        {
            throw new NotImplementedException();
        }
        public SPEvaluationPoints getFilterPoints(double lamda, FilterType filterType = FilterType.LowPass, FilterMethod filterMethod = FilterMethod.Gaussian)
        {
            throw new NotImplementedException();
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
            SPEvaluationPoints result = new SPEvaluationPoints();
            var points = line.FeatureAlignmentPoints;
            for (int i = 0; i < line.Deviations.Count; i++)
            {
                result.Add(new SPEvaluationPoint()
                {
                    rawPoint = points[i],
                    Deviation = line.Deviations[i],
                    SampleIndex = points[i].X,
                    Masked = false
                });
            }

            return result;
        }
        static public SPEvaluationPoints createFromCircle(SPCircle circle)
        {
            SPEvaluationPoints result = new SPEvaluationPoints();
            var points = circle.FeatureAlignmentPoints;
            for (int i = 0; i < circle.Deviations.Count; i++)
            {
                double angle = Math.Atan2(points[i].Y, points[i].X);
                if (angle < 0)
                    angle += Math.PI * 2;
                result.Add(new SPEvaluationPoint()
                {
                    rawPoint = points[i],
                    Deviation = circle.Deviations[i],
                    SampleIndex = angle,
                    Masked = false
                });
            }
            return result;
        }

        public SPEvaluationPoints Interpolate(double start, double end, double interval)
        {
            //1. sort the points by sample index
            //var sortedPoints = this.OrderBy(n => n.SampleIndex).ToList();
            //2. determine how many sections, and it's range of index 
            SPEvaluationSections sections = new SPEvaluationSections(this);
            //3. found out the average interval, as new interval 
            double approachInterval = sections.ApproachInterval;
            //4. interpolate for new points
            throw new NotImplementedException();
        }

      
        public double AverageInterval
        {
            get
            {
                double[] devs = new double[this.Count - 1];
                for (int i = 1; i < this.Count; i++)
                {
                    devs[i - 1] = this[i].SampleIndex - this[i - 1].SampleIndex;
                }
                return devs.Mean();
            }
        }
        public Tuple<double,double> Range
        {
            get
            {
                var sorted = this.OrderBy(n => n.SampleIndex);
                return new Tuple<double, double>(sorted.First().SampleIndex, sorted.Last().SampleIndex);
            }
        }
    }
}
