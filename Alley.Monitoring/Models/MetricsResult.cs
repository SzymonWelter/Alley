namespace Alley.Monitoring.Models
{
    public class MetricsResult
    {
        public string ResultType { get; set; }
        public MetricResult [] Result { get; set; }
    }
}