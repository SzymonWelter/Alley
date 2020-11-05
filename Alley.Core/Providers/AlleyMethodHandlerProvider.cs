using System;
using Alley.Definitions.Models.Interfaces;
using Grpc.Core;
using Grpc.Net.Client;

namespace Alley.Core.Providers
{
    public interface IAlleyMethodHandlerProvider
    {
        public UnaryServerMethod<IAlleyMessageModel, IAlleyMessageModel> GetUnaryHandler(
            Method<IAlleyMessageModel, IAlleyMessageModel> methodModel);

        public ClientStreamingServerMethod<IAlleyMessageModel, IAlleyMessageModel> GetClientStreamingHandler(
            Method<IAlleyMessageModel, IAlleyMessageModel> methodModel);

        public ServerStreamingServerMethod<IAlleyMessageModel, IAlleyMessageModel> GetServerStreamingHandler(
            Method<IAlleyMessageModel, IAlleyMessageModel> methodModel);

        public DuplexStreamingServerMethod<IAlleyMessageModel, IAlleyMessageModel> GetDuplexStreamingHandler(
            Method<IAlleyMessageModel, IAlleyMessageModel> methodModel);
    }

    public class AlleyMethodHandlerProvider : IAlleyMethodHandlerProvider
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