using System.Collections.Generic;

namespace Alley.Definitions.Models.Interfaces
{
    public interface IGrpcServiceDefinition
    {
        string Name { get;}
        IEnumerable<IGrpcMethodDefinition> Methods { get; }
    }
}