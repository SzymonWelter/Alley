using System.Collections.Generic;
using Alley.Definitions.Models.Interfaces;

namespace Alley.Definitions.Interfaces
{
    public interface IMicroservicesDefinitionsProvider
    {
        IEnumerable<IMicroserviceDefinition> GetMicroservicesDefinitions();
    }
}