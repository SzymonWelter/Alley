using Alley.Definitions.Models.Interfaces;
using Grpc.Core;

namespace Alley.Core.Factories
{
    public interface IMethodHandlerFactory
    {
        UnaryServerMethod<IAlleyMessageModel, IAlleyMessageModel> GetUnaryHandler(
            Method<IAlleyMessageModel, IAlleyMessageModel> method); 
        ClientStreamingServerMethod<IAlleyMessageModel, IAlleyMessageModel> GetClientStreamingServerHandler(
            Method<IAlleyMessageModel, IAlleyMessageModel> method);
        ServerStreamingServerMethod<IAlleyMessageModel, IAlleyMessageModel> GetServerStreamingServerHandler(
            Method<IAlleyMessageModel, IAlleyMessageModel> method);
        DuplexStreamingServerMethod<IAlleyMessageModel, IAlleyMessageModel> GetDuplexStreamingServerHandler(
            Method<IAlleyMessageModel, IAlleyMessageModel> method);
    }
}