using System;
using System.IO;
using System.IO.Abstractions;

namespace Alley.Configuration
{
    public interface IConfigurationProvider
    {
        IDirectoryInfo GetProtosLocalization();
        string ProtoPattern { get; }
    }
}