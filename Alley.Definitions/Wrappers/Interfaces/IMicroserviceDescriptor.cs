using System.Collections.Generic;
using System.IO;
using Alley.Definitions.Models.Interfaces;
using Alley.Utils.Models;

namespace Alley.Definitions.Wrappers.Interfaces
{
    public interface IMicroserviceDescriptor
    {
        IResult Read(string fileName, TextReader proto);
        IEnumerable<IGrpcServiceDefinition> GetServices();
    }
}