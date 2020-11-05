using System.Collections.Generic;
using Alley.Definitions.Models.Interfaces;

namespace Alley.Definitions.Models
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