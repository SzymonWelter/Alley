using System.Net;
using Alley.Definitions.Models;
using Alley.Utils.Models;

namespace Alley.Core
{
    public interface IMicroserviceManager
    {
        Microservice GetMicroserviceWithGrpcService(string grpcServiceName);
        Result RegisterMicroservice(Microservice microservice);
        Result UnregisterMicroservice(string microserviceName);
        Result RegisterInstance(string microServiceName, MicroServiceInstance instance);
        Result UnregisterInstance(string microserviceName, IPAddress address);
    }
}