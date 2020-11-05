using Grpc.Core;

namespace Alley.Definitions.Models.Interfaces
{
    public interface IGrpcMethodDefinition
    {
        string Name { get; }
        MethodType Type { get; }
    }
}