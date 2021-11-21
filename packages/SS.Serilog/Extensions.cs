using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace SS.Serilog
{
    public static class Extensions
    {
        
        public static void ConfigureLogging(HostBuilderContext hostBuilderContext, ILoggingBuilder logging)
        {
            var configuration = hostBuilderContext.Configuration;

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Environment", hostBuilderContext.HostingEnvironment.EnvironmentName)
                .Enrich.WithProperty("ApplicationName", hostBuilderContext.HostingEnvironment.ApplicationName)
                .CreateLogger();
            
            logging.ClearProviders();
            logging.AddSerilog(); 
        }

        public static void ConfigureLogging(WebHostBuilderContext hostBuilderContext, ILoggingBuilder logging)
        {
            var configuration = hostBuilderContext.Configuration;

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Environment", hostBuilderContext.HostingEnvironment.EnvironmentName)
                .Enrich.WithProperty("ApplicationName", hostBuilderContext.HostingEnvironment.ApplicationName)
                .CreateLogger();

            logging.ClearProviders();
            logging.AddSerilog();
        }

        public static IApplicationBuilder UseSerilogMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<SerilogMiddleware>();

            return app;
        }

        public static IServiceCollection AddSerilogMiddleware(this IServiceCollection services)
        {
            services.AddScoped<SerilogMiddleware>();
            
            return services;
        }

        public static IServiceCollection AddSerilogMiddleware(this IServiceCollection services, bool generateRCID)
        {
            services.Configure<SerilogMiddlewareOptions>(options =>
            {
                options.GenerateRCID = generateRCID;
            });
            AddSerilogMiddleware(services);
            return services;
        }
    }
}
