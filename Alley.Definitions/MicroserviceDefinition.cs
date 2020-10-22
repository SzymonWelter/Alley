using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.Reflection;

namespace Alley.Definitions
{
    internal class MicroserviceDefinition : IMicroserviceDefinition
    {
        public string Name { get; }
        public IEnumerable<IGrpcServiceDefinition> Services { get; }

        public MicroserviceDefinition(string name, IEnumerable<IGrpcServiceDefinition> services)
        {
            Name = name;
            Services = services;
        }
    }
}