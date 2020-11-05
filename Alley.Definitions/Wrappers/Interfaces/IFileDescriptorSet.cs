using System.Collections.Generic;
using System.IO;
using Google.Protobuf.Reflection;

namespace Alley.Definitions.Wrappers.Interfaces
{
    public interface IFileDescriptorSet
    {
        bool Add(string fileName, bool includedInOutput, TextReader textReader);
        void Process();
        IEnumerable<FileDescriptorProto> Files { get; }
    }
}