using System;
using System.Collections.Generic;
using System.Net;
using Alley.Utils.Models;

namespace Alley.Definitions.Models
{
    public class Microservice : IMicroService
    {
        public string Name { get; }

        private IProtoDescriptor _protosDescriptor;
        public Dictionary<IPAddress, MicroServiceInstance> _instances;
        
        public Microservice(string name, IProtoDescriptor protosDescriptor)
        {
            Name = name;
            _protosDescriptor = protosDescriptor;
        }
        
        public IEnumerable<GrpcService> GetServices()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MicroServiceInstance> GetInstances(string name)
        {
            throw new NotImplementedException();
        }

        public Result RegisterInstance(MicroServiceInstance instance)
        {
            throw new NotImplementedException();
        }

        public Result UnregisterInstance(IPAddress address)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object? obj)
        {
            return obj is Microservice candidate && candidate.Name == this.Name;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }

    public interface IProtoDescriptor
    {
        IEnumerable<GrpcService> GetServices();
    }

    public interface IMicroService
    {
        string Name { get; }
        IEnumerable<GrpcService> GetServices();
        IEnumerable<MicroServiceInstance> GetInstances(string name);
        Result RegisterInstance(MicroServiceInstance instance);
        Result UnregisterInstance(IPAddress address);
    }

    public class GrpcService
    {
        public string Name { get; set; }
        public IEnumerable<GrpcMethod> Methods { get; }
    }
    
    public class GrpcMethod
    {
        public string Name { get; }
    }
}