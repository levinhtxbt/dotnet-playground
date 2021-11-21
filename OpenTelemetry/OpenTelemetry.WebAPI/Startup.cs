using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenTelemetry.Trace;

namespace OpenTelemetry.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureOpenTelemetry(services);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OpenTelemetry.WebAPI", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OpenTelemetry.WebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public IServiceCollection ConfigureOpenTelemetry(IServiceCollection services)
        {

            services.AddOpenTelemetryTracing(
                (builder) => builder
                    .AddAspNetCoreInstrumentation(o => o.Filter =
                        (httpContext) =>
                        {
                            return !(httpContext.Request.Path.Value.EndsWith(".css") ||
                                     httpContext.Request.Path.Value.EndsWith(".js") ||
                                     httpContext.Request.Path.Value.EndsWith(".ico")||
                                     httpContext.Request.Path.Value.EndsWith("/swagger/index.html")||
                                     httpContext.Request.Path.Value.EndsWith("/swagger/v1/swagger.json"));
                        })
                    .AddHttpClientInstrumentation()
                    .AddSource("BackEndSource")
                    .SetSampler(new AlwaysOnSampler())
                    .AddZipkinExporter(option =>
                    {
                        //option.ServiceName = typeof(Startup).Assembly.GetName().Name;
                        option.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
                    })
            );

            return services;
        }
    }
}
