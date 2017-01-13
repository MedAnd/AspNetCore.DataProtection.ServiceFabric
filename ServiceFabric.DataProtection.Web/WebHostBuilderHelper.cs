using Microsoft.AspNetCore.Hosting;
using Microsoft.Net.Http.Server;
using System.Fabric;
using System.IO;

namespace ServiceFabric.DataProtection.Web
{
    /// <summary>
    /// 
    /// </summary>
    internal static class WebHostBuilderHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static IWebHost GetServiceFabricWebHost(ServerType serverType)
        {
            var endpoint = FabricRuntime.GetActivationContext().GetEndpoint("ServiceEndpoint");
            string serverUrl = $"{endpoint.Protocol}://{FabricRuntime.GetNodeContext().IPAddressOrFQDN}:{endpoint.Port}";

            return GetWebHost(endpoint.Protocol.ToString(), endpoint.Port.ToString(), serverType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="port"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static IWebHost GetWebHost(string protocol, string port, ServerType serverType)
        {
            switch (serverType)
            {
                case ServerType.WebListener:
                    {
                        IWebHostBuilder webHostBuilder = new WebHostBuilder()
                            .UseWebListener(options =>
                            {
                                options.ListenerSettings.Authentication.Schemes = AuthenticationSchemes.None;
                                options.ListenerSettings.Authentication.AllowAnonymous = true;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webHostBuilder"></param>
        /// <param name="protocol"></param>
        /// <param name="port"></param>
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

    /// <summary>
    /// 
    /// </summary>
    enum ServerType
    {
        Kestrel,
        WebListener
    }
}
