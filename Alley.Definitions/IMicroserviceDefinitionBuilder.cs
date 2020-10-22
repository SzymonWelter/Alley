using System;
using System.IO;
using System.IO.Abstractions;
using Alley.Utils;

namespace Alley.Definitions
{
    public interface IMicroserviceDefinitionBuilder
    {
        void AddProto(IFileInfo proto);
        IMicroserviceDefinition Build(string name);
    }
}