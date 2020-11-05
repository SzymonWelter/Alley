using System.Collections.Generic;

namespace Alley.Definitions.Models.Interfaces
{
    public interface IMicroserviceDefinition
    {
        string Name { get; }
        IEnumerable<IGrpcServiceDefinition> Services { get; }
    }
}