using Alley.Models;
using Alley.Utilities;
using Google.Protobuf.Reflection;

namespace Alley
{
    internal class MethodModelFactory
    {
        public AlleyMethodModel Create(FileDescriptorProto fileDescriptor, ServiceDescriptorProto service, MethodDescriptorProto method)
        {
            return Create(method.ClientStreaming, method.ServerStreaming, fileDescriptor.Package, service.Name,
                method.Name);
        }

        public AlleyMethodModel Create(bool clientStreaming, bool serverStreaming, string packageName,
            string serviceName, string methodName)
        {
            return new AlleyMethodModel(
                GrpcMethodHelper.GetMethodType(clientStreaming, serverStreaming),
                packageName,
                serviceName,
                methodName);
        }
    }
}