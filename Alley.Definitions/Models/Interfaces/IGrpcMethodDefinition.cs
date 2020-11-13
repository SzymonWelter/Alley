using Grpc.Core;

namespace Alley.Definitions.Models.Interfaces
{
    public interface IGrpcMethodDefinition
    {
        string ServiceName { get; }
        string Name { get; }
        MethodType Type { get; }
    }
}