using System;
using Alley.Utils;

namespace Alley.Context.Metrics
{
    public class ActiveConnectionMetric : IInstanceMetric
    {
        public ActiveConnectionMetric(double value)
        {
            Type = MetricType.ActiveConnection;
            Value = value;
        }

        public MetricType Type { get; }
        public double Value { get; }
        public int CompareTo(IInstanceMetric other)
        {
            if (other.Type != Type)
            {
                throw new ArgumentException(Messages.CanNotCompareMetrics(this, other));
            }
            return other.Value.CompareTo(Value);
        }

        public override string ToString()
        {
            return $"{Type}: {Value}";
        }
    }
}