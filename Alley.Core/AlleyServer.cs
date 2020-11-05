using System.Threading.Tasks;
using Grpc.Core;

namespace Alley.Core
{
    public class AlleyServer : IAlleyServer
    {
        private readonly Server _server;

        public AlleyServer(Server server)
        {
            _server = server;
        }
        public Task Run()
        {
            _server.Start();
            return _server.ShutdownTask;
        }
    }
}