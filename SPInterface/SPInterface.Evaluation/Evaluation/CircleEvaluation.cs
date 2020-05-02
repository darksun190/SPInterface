using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPInterface.Feature;
using SPInterface.Core;

namespace SPInterface.Evaluation
{
    /// <summary>
    /// 本项目中的“元素”并不是实际的几何元素，而是CALYPSO程序导出的，这些“元素”不应有粗差或滤波的操作
    /// 以扩展方法的Adapter实现
    /// </summary>
    internal static class CircleEvaluationAdapter
    {
        public static List<FeatureMeasurePoint> getOutlierPoints(this SPCircle circle, int factor)
        {
            return getOutlierPoints(circle, factor, factor);
        }
        public static List<FeatureMeasurePoint> getOutlierPoints(this SPCircle circle, int plusFactor, int minusFactor)
        {
            var sourcePoints = SPEvaluationPoints.createFromCircle(circle);
            var result = sourcePoints.getOutlierPoints(plusFactor, minusFactor).toFeatureMeasurePoints();
            return result;
        }
        public static List<FeatureMeasurePoint> getFilterPoints(this SPCircle circle, int cutoffRate, FilterType filterType = FilterType.LowPass, FilterMethod filterMethod = FilterMethod.Gaussian)
        {
            throw new NotImplementedException();
        }
    }
}
