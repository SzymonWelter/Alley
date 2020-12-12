using Alley.Definitions.Models.Interfaces;
using Google.Protobuf.Reflection;
using Grpc.Core;

namespace Alley.Definitions.Models
{
    internal class GrpcMethodDefinition : IGrpcMethodDefinition
    {
        public string ServiceName { get; }
        public string Name { get; }
        public MethodType Type { get; }
        
        public GrpcMethodDefinition(string serviceName, MethodDescriptorProto methodDescriptorProto)
        {
            ServiceName = serviceName;
            Name = methodDescriptorProto.Name;
            Type = methodDescriptorProto.ClientStreaming
                ? (methodDescriptorProto.ServerStreaming ? MethodType.DuplexStreaming : MethodType.ClientStreaming)
                : (methodDescriptorProto.ServerStreaming ? MethodType.ServerStreaming : MethodType.Unary);
        }
    }
}