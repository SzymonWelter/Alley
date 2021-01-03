using System.Net.Http;
using Alley.Utils;
using Alley.Utils.Configuration;

namespace Alley.Monitoring
{
    public class HealthFetcher : MetricFetcherBase, IHealthFetcher
    {
        public HealthFetcher(
            HttpClient httpClient, 
            IConfigurationProvider configurationProvider,
            IAlleyLogger logger) : 
            base(
                httpClient, 
                configurationProvider.HealthCheckQuery,
                logger)
        {
        }
    }
}