using System.Collections.Generic;

namespace Alley.Definitions
{
    public interface IMicroservicesDefinitionsProvider
    {
        IEnumerable<MicroserviceDefinition> GetMicroservicesDefinitions();
    }
}