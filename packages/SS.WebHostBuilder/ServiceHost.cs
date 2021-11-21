using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace SS.WebHostBuilder
{
    public class ServiceHost
    {
        private readonly IHost _host;

        public ServiceHost(IHost webHost)
        {
            _host = webHost;
        }

        public void Run() => _host.Run();
    }
}
