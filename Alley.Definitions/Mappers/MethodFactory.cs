using Alley.Definitions.Mappers.Interfaces;
using Alley.Definitions.Models.Interfaces;
using Alley.Serialization;
using Alley.Serialization.Models;
using Alley.Utils.Helpers;
using Grpc.Core;

namespace Alley.Definitions.Mappers
{
    public class MethodFactory : IMethodFactory<IAlleyMessageModel, IAlleyMessageModel>
    {
        public Method<IAlleyMessageModel, IAlleyMessageModel> Create(IGrpcMethodDefinition methodDefinition)
        {
            return new Method<IAlleyMessageModel, IAlleyMessageModel>(
                methodDefinition.Type,
                methodDefinition.ServiceName,
                methodDefinition.Name,
                AlleyMessageSerializer.AlleyMessageMarshaller,
                AlleyMessageSerializer.AlleyMessageMarshaller
            );
        }

        public Method<IAlleyMessageModel, IAlleyMessageModel> Create(string methodFullName, MethodType methodType)
        {
            MethodHelper.SplitMethodFullName(methodFullName, out var serviceName, out var methodName);
            return new Method<IAlleyMessageModel, IAlleyMessageModel>(
                methodType,
                serviceName,
                methodName,
                AlleyMessageSerializer.AlleyMessageMarshaller,
                AlleyMessageSerializer.AlleyMessageMarshaller
            );
        }
    }
}