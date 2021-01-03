using Alley.Context.Models.Interfaces;
using Alley.Monitoring.Models;

namespace Alley.Monitoring
{
    public interface IMetricMetadataFactory
    {
        MetricMetadata GetFrom(MetricResult metricResult);
        MetricMetadata GetFrom(IReadonlyMicroserviceInstance microserviceInstance);
    }
}