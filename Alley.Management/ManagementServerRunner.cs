using Microsoft.Extensions.Hosting;

namespace Alley.Management
{
    public class ManagementServerRunner : IManagementServerRunner
    {
        private IHost _host;

        public void SetHost(IHost host)
        {
            _host = host;
        }
        public void Run()
        {
            _host.Run();
        }
    }
}