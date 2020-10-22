using System.Collections.Generic;

namespace Alley.Definitions
{
    public interface IGrpcServiceDefinition
    {
        string Name { get;}
        IEnumerable<IGrpcMethodDefinition> Methods { get; }
    }
}