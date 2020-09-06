using Grpc.Core;

namespace Alley.Models
{
    public class AlleyMethodModel
    {
        public MethodType MethodType { get; }
        public string PackageName { get; }
        public string ServiceName { get; }
        public string MethodName { get; }
        
        public AlleyMethodModel(MethodType methodType, string packageName, string serviceName, string methodName)
        {
            MethodType = methodType;
            PackageName = packageName;
            ServiceName = serviceName;
            MethodName = methodName;
        }
    }
}