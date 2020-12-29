namespace Alley.Context.Metrics
{
    public class MemoryUsageMetric : IInstanceMetric
    {
        public double Value { get; }
        public MetricType Type { get; }
        public MemoryUsageMetric(double value)
        {
            Value = value;
            Type = MetricType.ProcessorUsage;
        }

        public int CompareTo(IInstanceMetric other)
        {
            return Value.CompareTo(other.Value);
        }

        public override string ToString()
        {
            return $"{{{Type.ToString()} : {Value}}}";
        }
    }
}