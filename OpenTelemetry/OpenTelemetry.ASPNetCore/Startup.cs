using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenTelemetry.ASPNetCore.Services;
using OpenTelemetry.Trace;

namespace OpenTelemetry.ASPNetCore
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
            ConfigureBackendAPI(services);
            ConfigureOpenTelemetry(services);

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public IServiceCollection ConfigureBackendAPI(IServiceCollection services)
        {
            var configureClient = new Action<IServiceProvider, HttpClient>(async (provider, client) =>
            {
                //var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
                //var accessToken = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");

                client.BaseAddress = Configuration.GetServiceUri("backend");
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpClient<IWeatherForecastApiService, WeatherForecastApiService>(configureClient);

            return services;

        }

        public IServiceCollection ConfigureOpenTelemetry(IServiceCollection services)
        {

            services.AddOpenTelemetryTracing(
                (builder) => builder
                    .AddAspNetCoreInstrumentation(o => o.Filter =
                        (httpContext) =>
                        {
                            return !(httpContext.Request.Path.Value.EndsWith(".css") ||
                                     httpContext.Request.Path.Value.EndsWith(".js")  ||
                                     httpContext.Request.Path.Value.EndsWith(".ico") ||
                                     httpContext.Request.Path.Value.EndsWith(".png"));
                        })
                    .AddHttpClientInstrumentation()
                    .AddSource("FrontEndSource")
                    .SetSampler(new AlwaysOnSampler())
                    .AddZipkinExporter(option =>
                    {
                        option.ServiceName = typeof(Startup).Assembly.GetName().Name;
                        option.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
                    })
            );

            return services;
        }
    }
}
