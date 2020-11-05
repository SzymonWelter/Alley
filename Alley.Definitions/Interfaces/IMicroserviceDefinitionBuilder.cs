using System.IO.Abstractions;
using Alley.Definitions.Models.Interfaces;

namespace Alley.Definitions.Interfaces
{
    public interface IMicroserviceDefinitionBuilder
    {
        void AddProto(IFileInfo proto);
        IMicroserviceDefinition Build(string name);
    }
}