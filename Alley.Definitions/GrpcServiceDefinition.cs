using System.Collections.Generic;
using Google.Protobuf.Reflection;

namespace Alley.Definitions
{
    internal class GrpcServiceDefinition : IGrpcServiceDefinition
    {
        public string Name { get; }
        public IEnumerable<IGrpcMethodDefinition> Methods { get; }
        
        public GrpcServiceDefinition(FileDescriptorProto file)
        {
        }
    }
}