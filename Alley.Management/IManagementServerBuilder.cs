using Microsoft.Extensions.Hosting;

namespace Alley.Management
{
    public interface IManagementServerBuilder
    {
        IHost Build();
    }
}