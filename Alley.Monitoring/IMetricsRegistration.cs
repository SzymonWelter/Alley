using System.Threading.Tasks;

namespace Alley.Monitoring
{
    public interface IMetricsRegistration
    {
        Task Start();
    }
}