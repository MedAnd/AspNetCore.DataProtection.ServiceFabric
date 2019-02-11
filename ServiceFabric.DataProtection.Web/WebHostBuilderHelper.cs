using Microsoft.AspNetCore.Hosting;
using Microsoft.Net.Http.Server;
using System.Fabric;
using System.IO;
using Microsoft.AspNetCore.Server.HttpSys;

namespace ServiceFabric.DataProtection.Web
{
    internal static class WebHostBuilderHelper
    {
        public static IWebHost GetServiceFabricWebHost(ServerType serverType)
        {
            var endpoint = FabricRuntime.GetActivationContext().GetEndpoint("ServiceEndpoint");
            string serverUrl = $"{endpoint.Protocol}://{FabricRuntime.GetNodeContext().IPAddressOrFQDN}:{endpoint.Port}";

            return GetWebHost(endpoint.Protocol.ToString(), endpoint.Port.ToString(), serverType);
        }
        
        public static IWebHost GetWebHost(string protocol, string port, ServerType serverType)
        {
            switch (serverType)
            {
                case ServerType.WebListener:
                    {
                        IWebHostBuilder webHostBuilder = new WebHostBuilder()
                            .UseHttpSys(options =>
                            {
                                options.Authentication.Schemes = Microsoft.AspNetCore.Server.HttpSys.AuthenticationSchemes.None;
                                options.Authentication.AllowAnonymous = true;
                            });

                        return ConfigureWebHostBuilder(webHostBuilder, protocol, port);
                    }
                case ServerType.Kestrel:
                    {
                        IWebHostBuilder webHostBuilder = new WebHostBuilder();
                        webHostBuilder.UseKestrel();

                        return ConfigureWebHostBuilder(webHostBuilder, protocol, port);
                    }
                default:
                    return null;
            }
        }

        static IWebHost ConfigureWebHostBuilder(IWebHostBuilder webHostBuilder, string protocol, string port)
        {
            return webHostBuilder                
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseWebRoot(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
                .UseStartup<Startup>()
                .UseUrls($"{protocol}://+:{port}")
                .Build();
        }
    }
    
    enum ServerType
    {
        Kestrel,
        WebListener,

    }
}
