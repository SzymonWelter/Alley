using Alley.Core.Models;
using Alley.Core.Utilities;
using Google.Protobuf.Reflection;

namespace Alley.Core.Factories
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
                MethodTypeHelper.GetMethodType(clientStreaming, serverStreaming),
                packageName,
                serviceName,
                methodName);
        }
    }
}