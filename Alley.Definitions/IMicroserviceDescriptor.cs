using System.Collections.Generic;
using System.IO;
using Alley.Utils;

namespace Alley.Definitions
{
    public interface IMicroserviceDescriptor
    {
        IResult Read(string fileName, Stream proto);
        IEnumerable<IGrpcServiceDefinition> GetServices();
    }
}