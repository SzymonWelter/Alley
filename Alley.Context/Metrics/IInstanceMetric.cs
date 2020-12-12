using System;

namespace Alley.Context.Metrics
{
    public interface IInstanceMetric : IComparable<IInstanceMetric>
    {
        public double Value { get; }
        public MetricType Type { get; }
    }
}