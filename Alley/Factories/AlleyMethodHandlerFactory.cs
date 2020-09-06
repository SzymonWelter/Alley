using System;
using System.Threading.Tasks;
using Alley.Models;
using Grpc.Core;

namespace Alley
{
    internal class AlleyMethodHandlerFactory
    {
        public UnaryServerMethod<IAlleyMessageModel,IAlleyMessageModel> GetUnaryHandler()
        {
            throw new NotImplementedException();
        }

        public ClientStreamingServerMethod<IAlleyMessageModel, IAlleyMessageModel> GetClientStreamingHandler()
        {
            throw new NotImplementedException();
        }

        public ServerStreamingServerMethod<IAlleyMessageModel, IAlleyMessageModel> GetServerStreamingHandler()
        {
            throw new NotImplementedException();
        }

        public DuplexStreamingServerMethod<IAlleyMessageModel, IAlleyMessageModel> GetDuplexStreamingHandler()
        {
            throw new NotImplementedException();
        } 
    }
}