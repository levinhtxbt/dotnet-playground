using System;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransit.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddMessageQueue(
            this IServiceCollection services,
            Action<IServiceCollectionBusConfigurator> configure = default,
            Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator> rabbitMq = default)
        {
            services.AddMassTransit(x =>
            {
                //x.UsingRabbitMq(ConfigureBus);
                // x.UsingInMemory();
                x.UsingInMemory((context, cfg) =>
               {
                   cfg.TransportConcurrencyLimit = 100;

                   cfg.ConfigureEndpoints(context);
               });
                configure?.Invoke(x);
            });
            services.AddMassTransitHostedService();

            return services;
        }
        public static IServiceCollection AddMessageQueueRabbitMq(
                   this IServiceCollection services,
                   Action<IServiceCollectionBusConfigurator> configure = default,
                   Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator> rabbitMq = default)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq(ConfigureBus);
                configure?.Invoke(x);
            });
            services.AddMassTransitHostedService();

            return services;
        }

        public static IRabbitMqBusFactoryConfigurator CreateDefaultBusFactoryConfigurator(
            this IRabbitMqBusFactoryConfigurator configurator,
            IConfiguration configuration)
        {
            var options = configuration.GetMassTransitSettings();

            if (!string.IsNullOrEmpty(options.AWSMQ))
            {
                configurator.Host(new Uri(options.AWSMQ), h =>
                {
                    h.Username(options.Username);
                    h.Password(options.Password);
                });

            }
            else
            {
                configurator.Host(options.Host, options.VirtualHost, h =>
                {
                    h.Username(options.Username);
                    h.Password(options.Password);
                });
            }

            return configurator;

        }

        private static void ConfigureBus(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator configurator)
        {
            var configuration = context.GetService<IConfiguration>();
            var options = configuration.GetMassTransitSettings();
            if (!string.IsNullOrEmpty(options.AWSMQ))
            {
                configurator.Host(new Uri(options.AWSMQ), h =>
                {
                    h.Username(options.Username);
                    h.Password(options.Password);
                });

            }
            else
            {
                configurator.Host(options.Host, options.VirtualHost, h =>
                {
                    h.Username(options.Username);
                    h.Password(options.Password);
                });
            }
            configurator.ConfigureEndpoints(context);

        }

    }
}