using System.Collections.Generic;

namespace Alley.Definitions
{
    public interface IMicroserviceDefinition
    {
        string Name { get; }
        IEnumerable<IGrpcServiceDefinition> Services { get; }
    }
}