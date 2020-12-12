using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Alley.Definitions.Interfaces;
using Alley.Definitions.Models;
using Alley.Definitions.Models.Interfaces;
using Alley.Definitions.Wrappers.Interfaces;
using Alley.Utils;
using Alley.Utils.Models;
using Google.Protobuf.Reflection;

namespace Alley.Definitions.Wrappers
{
    internal class MicroserviceDescriptor : IMicroserviceDescriptor
    {
        private readonly IFileDescriptorSet _fileDescriptorSet;

        public MicroserviceDescriptor(IFileDescriptorSet fileDescriptorSet)
        {
            _fileDescriptorSet = fileDescriptorSet;
        }
        public IResult Read(string fileName, TextReader proto)
        {
            try
            {
                var isSuccess = _fileDescriptorSet.Add(fileName, true, proto);
                return isSuccess ? 
                    Result.Success(Messages.FileHasBeenAddedToDescriptor(fileName)) : 
                    Result.Failure(Messages.FileCanNotBeAddedToDescriptor(fileName));
            }
            catch (Exception e)
            {
                return Result.Failure(e.Message);
            }
        }

        public IEnumerable<IGrpcServiceDefinition> GetServices()
        {
            _fileDescriptorSet.Process();
            return _fileDescriptorSet.Files.SelectMany(GetServicesFromProto);
        }

        private IEnumerable<IGrpcServiceDefinition> GetServicesFromProto(FileDescriptorProto file)
        {
            return file.Services.Select(s => new GrpcServiceDefinition(s, file.Package));
        }
    }
}