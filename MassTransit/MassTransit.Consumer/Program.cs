using MassTransit.Consumer.Consumer;
using MassTransit.Core;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MassTransit.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            new HostBuilder()
                .ConfigureAppConfiguration(config =>
                {
                    config.AddEnvironmentVariables();
                    config.AddJsonFile("appsettings.json", true);
                })
                .ConfigureLogging((context, builder) =>
                {
                    builder.AddConfiguration(context.Configuration.GetSection("Logging"));
                    builder.AddConsole();
                })
                .ConfigureServices(services =>
                {
                    services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
                    services.AddMessageQueueRabbitMq(x =>
                    {
                        x.AddConsumer<OrderSubmitConsumer>();
                    });
                })
                .Build()
                .Run();
        }
    }
}