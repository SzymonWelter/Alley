using System.Net;
using Alley.Definitions.Models;

namespace Alley.Core
{
    public class MicroServiceInstance
    {
        public Microservice Microservice { get; }
        public IPAddress IpAddress { get; }

        public MicroServiceInstance(Microservice microservice, IPAddress ipAddress)
        {
            Microservice = microservice;
            IpAddress = ipAddress;
        }

        public override bool Equals(object? obj)
        {
            return obj is MicroServiceInstance instance && instance.IpAddress == this.IpAddress;
        }

        public override int GetHashCode()
        {
            return IpAddress.GetHashCode();
        }
    }
}