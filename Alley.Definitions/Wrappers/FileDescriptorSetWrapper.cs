using System.Collections.Generic;
using System.IO;
using Alley.Definitions.Interfaces;
using Alley.Definitions.Wrappers.Interfaces;
using Google.Protobuf.Reflection;

namespace Alley.Definitions.Wrappers
{
    public class FileDescriptorSetWrapper : IFileDescriptorSet
    {
        private readonly FileDescriptorSet _fileDescriptorSet;

        public FileDescriptorSetWrapper(FileDescriptorSet fileDescriptorSet)
        {
            _fileDescriptorSet = fileDescriptorSet;
        }

        public bool Add(string fileName, bool includedInOutput, TextReader textReader) =>
            _fileDescriptorSet.Add(fileName, includedInOutput, textReader);

        public void Process() =>
            _fileDescriptorSet.Process();

        public IEnumerable<FileDescriptorProto> Files => _fileDescriptorSet.Files;
    }
}