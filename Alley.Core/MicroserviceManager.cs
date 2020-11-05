using System;
using System.Collections.Concurrent;
using System.Net;
using Alley.Definitions.Models;
using Alley.Utils.Models;

namespace Alley.Core
{
    public class MicroserviceManager : IMicroserviceManager
    {
        private static readonly Lazy<MicroserviceManager> _instance = new Lazy<MicroserviceManager>(() => new MicroserviceManager());
        public static MicroserviceManager Instance => _instance.Value;

        private readonly ConcurrentDictionary<string, Microservice> _microservices = new ConcurrentDictionary<string, Microservice>();

        private MicroserviceManager()
        {
            
        }

        public Microservice GetMicroserviceWithGrpcService(string grpcServiceName)
        {
            throw new NotImplementedException();
        }

        public Result RegisterMicroservice(Microservice microservice)
        {
            throw new NotImplementedException();
        }

        public Result UnregisterMicroservice(string microserviceName)
        {
            throw new NotImplementedException();
        }

        public Result RegisterInstance(string microServiceName, MicroServiceInstance instance)
        {
            throw new NotImplementedException();
        }

        public Result UnregisterInstance(string microserviceName, IPAddress address)
        {
            throw new NotImplementedException();
        }
    }
}