using System.IO.Abstractions;

namespace Alley.Utils.Configuration
{
    public interface IConfigurationProvider
    {
        IDirectoryInfo GetProtosLocalization();
        string ProtoPattern { get; }
    }
}