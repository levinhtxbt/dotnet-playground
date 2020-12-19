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
using AwesomeShop.Identity.Data;
using AwesomeShop.Identity.IdentityServer;
using AwesomeShop.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AwesomeShop.Identity
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "awesomeCors";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var clientUrls = new Dictionary<string, string>
            {
                ["Mvc"] = Configuration["ClientUrl:Mvc"],
                ["Swagger"] = Configuration["ClientUrl:Swagger"],
                ["Angular"] = Configuration["ClientUrl:Angular"],
            };

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();


            services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddInMemoryIdentityResources(IdentityServerConfig.Ids)
                .AddInMemoryApiResources(IdentityServerConfig.Apis)
                .AddInMemoryClients(IdentityServerConfig.Clients(clientUrls))
                .AddAspNetIdentity<User>()
                .AddDeveloperSigningCredential(); // not recommended for production - you need to store your key material somewhere secure

            services.AddAuthentication()
                .AddLocalApi("Bearer", option =>
                {
                    option.ExpectedScope = "api.awesome.shop";
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Bearer", policy =>
                {
                    policy.AddAuthenticationSchemes("Bearer");
                    policy.RequireAuthenticatedUser();
                });
            });
            
            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins(clientUrls["Mvc"], clientUrls["Angular"])
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });


            services.AddControllersWithViews();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Awesome Shop API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri("/connect/token", UriKind.Relative),
                            AuthorizationUrl = new Uri("/connect/authorize", UriKind.Relative),
                            Scopes = new Dictionary<string, string> { { "api.awesome.shop", "My Shop API" } }
                        },
                    },
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>{ "api.awesome.shop" }
                    }
                });
            });

            //services.AddDatabaseDeveloperPageExceptionFilter();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSwagger();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseSwaggerUI(c =>
            {
                c.OAuthClientId("swagger");
                c.OAuthClientSecret("secret");
                c.OAuthUsePkce();
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Awesome Shop API V1");
            });
            app.UseCors(MyAllowSpecificOrigins);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
