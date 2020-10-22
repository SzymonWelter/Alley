using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Alley.Utils;
using Google.Protobuf.Reflection;

namespace Alley.Definitions
{
    internal class MicroserviceDescriptor : IMicroserviceDescriptor
    {
        private readonly FileDescriptorSet _fileDescriptorSet= new FileDescriptorSet();
        public IResult Read(string fileName, Stream proto)
        {
            try
            {
                using var streamReader = new StreamReader(proto);
                var isSuccess = _fileDescriptorSet.Add(fileName, true, streamReader);
                return Result.Determine(
                    isSuccess, 
                    Messages.FileCanNotBeAddedToDescriptor(fileName));
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
            return file.Services.Select(s => new GrpcServiceDefinition(file));
        }
    }
}