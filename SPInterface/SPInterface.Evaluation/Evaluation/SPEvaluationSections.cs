using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPInterface.Evaluation.Evaluation
{
    class SPEvaluationSections : List<SPEvaluationPoints>
    {
        private SPEvaluationPoints source;
        private SPEvaluationPoints sortedPoints;
        public SectionsIndex Indecies
        {
            get
            {
                SectionsIndex result = new SectionsIndex();
                foreach(var s in this)
                {
                    result.Add(s.Range);
                }
                return result;
            }
        }

        public SPEvaluationSections(SPEvaluationPoints sourcePoints)
        {
            var result = new List<SPEvaluationPoints>();
            const int N = 5;
            const double distanceFactor = 10.0;
            source = sourcePoints;
            int pointIndex = 0;
            bool isNewSection = true;
            sortedPoints = source.OrderBy(n => n.SampleIndex).ToList() as SPEvaluationPoints;
            while (pointIndex < sortedPoints.Count)
            {
                if (isNewSection)
                {

                    //get first N points as new sction base
                    if (pointIndex + N > sortedPoints.Count)
                    {
                        break;
                    }
                    SPEvaluationPoints section = new SPEvaluationPoints();
                    section.AddRange(sortedPoints.GetRange(pointIndex, N));
                    result.Add(section);
                    isNewSection = false;
                    pointIndex += N;
                }
                else
                {
                    var section = result.Last();
                    double currentInterval = section.AverageInterval;
                    var currentPoint = sortedPoints[pointIndex];
                    var lastPoint = section.Last();
                    if (currentPoint.SampleIndex - lastPoint.SampleIndex > distanceFactor * currentInterval)
                    {
                        // turn to next section
                        isNewSection = true;
                        continue;
                    }
                    else
                    {
                        //insert point to last section
                        section.Add(currentPoint);
                        pointIndex++;
                    }
                }
            }
        }

        public double ApproachInterval
        {
            get
            {
                return this.Average(n => n.AverageInterval);
            }
        }
        internal class SectionsIndex : List<Tuple<double, double>>
        {
            public new void Add(Tuple<double, double> item)
            {
                bool ValidationnStatus = item.Item2 > item.Item1 && item.Item1 > this.Last().Item2;
                if (ValidationnStatus)
                    base.Add(item);
                else
                    throw new FormatException("index should be ascending listed.");
            }
            public void Add(double item1, double item2)
            {
                Add(new Tuple<double, double>(item1, item2));
            }

        }
    }
}
