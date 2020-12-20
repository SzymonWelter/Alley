using System.IO.Abstractions;

namespace Alley.Utils.Configuration
{
    public interface IConfigurationProvider
    {
        IDirectoryInfo GetProtosPath();
        string ProtoPattern { get; }
        string Protocol { get; }
    }
}