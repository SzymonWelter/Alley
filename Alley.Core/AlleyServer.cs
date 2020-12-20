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
        public void Run()
        {
            _server.Start();
            _server.ShutdownTask.GetAwaiter().GetResult();
        }
    }
}