using System.Collections.Generic;

namespace Alley.Monitoring
{
    public interface IFetchersProvider
    {
        IEnumerable<IMetricFetcher> GetFetchers();
    }
}