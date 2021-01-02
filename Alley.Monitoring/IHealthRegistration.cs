using System.Threading.Tasks;

namespace Alley.Monitoring
{
    public interface IHealthRegistration
    {
        Task Start();
    }
}