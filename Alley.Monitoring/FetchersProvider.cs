using System.Collections.Generic;

namespace Alley.Monitoring
{
    public class FetchersProvider : IFetchersProvider
    {
        private readonly CpuUsageFetcher _usageFetcher;

        public FetchersProvider(CpuUsageFetcher usageFetcher)
        {
            _usageFetcher = usageFetcher;
        }
        public IEnumerable<IMetricFetcher> GetFetchers()
        {
            yield return _usageFetcher;
        }
    }
}