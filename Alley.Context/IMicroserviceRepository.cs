using System.Collections.Generic;
using System.Net;
using Alley.Context.Models.Interfaces;
using Alley.Utils.Models;

namespace Alley.Context
{
    public interface IMicroserviceRepository<TRequest,TResponse> : IReadonlyMicroserviceRepository
    {
        Result RegisterInstance(string serviceName, IMicroserviceInstance microserviceInstance);
        Result<IMicroserviceInstance> UnregisterInstance(string serviceName, IPAddress address);
    }

    public interface IReadonlyMicroserviceRepository
    {
        Result<IEnumerable<IMicroserviceInstance>> GetInstances(string serviceName);
    }
}