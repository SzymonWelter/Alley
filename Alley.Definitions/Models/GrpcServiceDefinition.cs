﻿using System.Collections.Generic;
using System.Linq;
using Alley.Definitions.Models.Interfaces;
using Alley.Utils.Helpers;
using Google.Protobuf.Reflection;

namespace Alley.Definitions.Models
{
    internal class GrpcServiceDefinition : IGrpcServiceDefinition
    {
        public string Name { get; }
        public IEnumerable<IGrpcMethodDefinition> Methods { get; }
        
        public GrpcServiceDefinition(ServiceDescriptorProto serviceDescriptor, string package)
        {
            Name = FormatHelper.FormatServiceFullName(package, serviceDescriptor.Name);
            Methods = serviceDescriptor.Methods.Select(m => new GrpcMethodDefinition(Name, m));
        }
    }
}