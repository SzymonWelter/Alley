using Alley.Utils.Models;

namespace Alley.Context.Models.Interfaces
{
    public interface IMicroservice<TMethod>
    {
        Result RegisterInstance(IMicroserviceInstance microServiceInstance);
        Result<TMethod> GetMethod(string methodName);
    }
}