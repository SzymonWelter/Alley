namespace Alley.Monitoring.Models
{
    public class MetricResult
    {
        public MetricMetadata Metric { get; set; }
        public double [] Value { get; set; }
    }
}