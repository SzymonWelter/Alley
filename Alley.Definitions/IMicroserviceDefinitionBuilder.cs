using System.IO;

namespace Alley.Definitions
{
    public interface IMicroserviceDefinitionBuilder
    {
        void AddProto(Stream proto);
        MicroserviceDefinition Build(string name);
    }
}