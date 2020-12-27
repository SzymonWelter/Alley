using System.Net.Http;
using Alley.Utils.Configuration;

namespace Alley.Monitoring
{
    public class HealthFetcher : MetricFetcherBase, IHealthFetcher
    {
        private readonly string _query;
        public HealthFetcher(
            HttpClient httpClient, 
            IConfigurationProvider configurationProvider) : 
            base(
                httpClient, 
                configurationProvider.HealthCheckQuery)
        {
        }
    }
}