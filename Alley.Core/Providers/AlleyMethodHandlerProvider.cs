using System;
using Alley.Core.Models;
using Grpc.Core;
using Grpc.Net.Client;

namespace Alley.Core.Providers
{
    internal class AlleyMethodHandlerProvider
    {
        public UnaryServerMethod<IAlleyMessageModel, IAlleyMessageModel> GetUnaryHandler(
            Method<IAlleyMessageModel, IAlleyMessageModel> methodModel)
        {
            return async (request, context) =>
            {
                var channel = GrpcChannel.ForAddress("http://localhost:6000");
                var callInvoker = channel.CreateCallInvoker();
                var result = await callInvoker.AsyncUnaryCall(
                    methodModel, 
                    "http://localhost:6000", 
                    new CallOptions(
                        context.RequestHeaders,
                        context.Deadline,
                        context.CancellationToken,
                        context.WriteOptions,
                        context.CreatePropagationToken()),
                    request);
                return result;
            };
        }

        public ClientStreamingServerMethod<IAlleyMessageModel, IAlleyMessageModel> GetClientStreamingHandler(
            Method<IAlleyMessageModel, IAlleyMessageModel> methodModel)
        {
            throw new NotImplementedException();
        }

        public ServerStreamingServerMethod<IAlleyMessageModel, IAlleyMessageModel> GetServerStreamingHandler(
            Method<IAlleyMessageModel, IAlleyMessageModel> methodModel)
        {
            throw new NotImplementedException();
        }

        public DuplexStreamingServerMethod<IAlleyMessageModel, IAlleyMessageModel> GetDuplexStreamingHandler(
            Method<IAlleyMessageModel, IAlleyMessageModel> methodModel)
        {
            throw new NotImplementedException();
        } 
    }
}