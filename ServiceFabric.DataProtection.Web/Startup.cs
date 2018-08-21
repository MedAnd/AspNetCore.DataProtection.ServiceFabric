using AspNetCore.DataProtection.ServiceFabric.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSwag.AspNetCore;
using System.Reflection;
using NJsonSchema;
using System.Fabric;

namespace ServiceFabric.DataProtection.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            string dataProtectionServiceUri = string.Empty;
            try
            {
                dataProtectionServiceUri = $"{FabricRuntime.GetActivationContext().ApplicationName}/DataProtectionService";
            }
            catch
            {
                dataProtectionServiceUri = "fabric:/ServiceFabric.DataProtection/DataProtectionService";
            }

            // Add Service Fabric DataProtection
            services.AddDataProtection()
                    .SetApplicationName("ServiceFabric-DataProtection-Web")
                    .PersistKeysToServiceFabric(dataProtectionServiceUri);

            services.AddSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            // Enable the Swagger UI middleware and the Swagger generator
            app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, settings =>
            {
                settings.DocExpansion = "full";
                settings.GeneratorSettings.DefaultPropertyNameHandling = PropertyNameHandling.CamelCase;
                settings.GeneratorSettings.IsAspNetCore = true;
                settings.GeneratorSettings.Title = "AspNetCore.DataProtection.ServiceFabric API";
                settings.GeneratorSettings.Version = "v1.05";
                settings.GeneratorSettings.Description = "A cross platform & ready to use ASP.NET Core DataProtection Service Fabric microservice, " +
                    "used for protecting and unprotecting sensitive data such as tokens and cookies in a multi-node & load balanced environment.";

                settings.PostProcess = doc =>
                {
                    doc.Info.Contact = new NSwag.SwaggerContact
                    {
                        Url = "https://github.com/MedAnd/AspNetCore.DataProtection.ServiceFabric"
                    };
                };
            });

            app.UseMvc();
        }
    }
}
