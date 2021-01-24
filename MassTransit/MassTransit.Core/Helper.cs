using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransit.Core
{
    public static class Helpers
    {
        public static MassTransitSettings GetMassTransitSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new MassTransitSettings();

            var configurationSection = configuration.GetSection("MassTransit");
            configurationSection.Bind(options);
            services.Configure<MassTransitSettings>(configurationSection);

            return options;
        }


        public static MassTransitSettings GetMassTransitSettings(this IConfiguration configuration)
        {
            var options = new MassTransitSettings();
            var configurationSection = configuration.GetSection("MassTransit");
            configurationSection.Bind(options);

            return options;

        }
    }
}