using System.Collections.Generic;
using Alley.Definitions.Models.Interfaces;
using Google.Protobuf.Reflection;

namespace Alley.Definitions.Models
{
    internal class GrpcServiceDefinition : IGrpcServiceDefinition
    {
        public string Name { get; }
        public IEnumerable<IGrpcMethodDefinition> Methods { get; }
        
        public GrpcServiceDefinition(ServiceDescriptorProto serviceDescriptor)
        {
            Name = serviceDescriptor.Name;
        }
    }
}