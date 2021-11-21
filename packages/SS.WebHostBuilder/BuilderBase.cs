using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SS.WebHostBuilder
{
    public abstract class BuilderBase
    {
        public abstract ServiceHost Build();
    }

    public class HostBuilder : BuilderBase
    {
        private readonly IHostBuilder _hostBuilder;

        public HostBuilder(IHostBuilder webHostBuilder)
        {
            _hostBuilder = webHostBuilder;
        }

        public static HostBuilder Create<TStartup>(string[] args) where TStartup : class
        {
            Console.Title = typeof(TStartup).Namespace;

            var webHostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TStartup>();
                });

            return new HostBuilder(webHostBuilder);
        }

        public HostBuilder UseContentRoot()
        {
            _hostBuilder.UseContentRoot(Directory.GetCurrentDirectory());

            return this;
        }

        public HostBuilder UseDefaultAppConfiguration()
        {
            _hostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;
                config
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
            });

            return this;
        }

        public HostBuilder UseOcelot()
        {
            _hostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;
                config
                    .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                    .AddJsonFile("ocelot.json")
                    .AddJsonFile($"ocelot.{env.EnvironmentName}.json", true, true)
                    .AddEnvironmentVariables();
            });

            return this;
        }

        public HostBuilder UseSeriLog()
        {
            _hostBuilder.ConfigureLogging(SS.Serilog.Extensions.ConfigureLogging);

            return this;
        }

        public HostBuilder UseAutofac()
        {
            _hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            return this;
        }

        public override ServiceHost Build()
        {
            return new ServiceHost(_hostBuilder.Build());
        }
    }
}
