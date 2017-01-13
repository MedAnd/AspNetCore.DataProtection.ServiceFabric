using CommandLine;
using Microsoft.AspNetCore.Hosting;
using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Threading;

namespace ServiceFabric.DataProtection.Web
{
    /// <summary>
    /// 
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        public static void Main(string[] args)
        {
            var parser = new Parser(with => 
            {
                with.EnableDashDash = true;
                with.HelpWriter = Console.Out;
            });

            var result = parser.ParseArguments<Options>(args);

            result.MapResult(options =>
            {
                switch (options.Host.ToLower())
                {
                    case "servicefabric-weblistener":
                        {
                            ServiceRuntime.RegisterServiceAsync("WebServiceType", context => new WebService(context, ServerType.WebListener)).GetAwaiter().GetResult();
                            Thread.Sleep(Timeout.Infinite);
                            break;
                        }
                    case "servicefabric-kestrel":
                        {
                            ServiceRuntime.RegisterServiceAsync("WebServiceType", context => new WebService(context, ServerType.Kestrel)).GetAwaiter().GetResult();
                            Thread.Sleep(Timeout.Infinite);
                            break;
                        }
                    case "weblistener":
                        {
                            using (var host = WebHostBuilderHelper.GetWebHost(options.Protocol, options.Port, ServerType.WebListener))
                            {
                                host.Run();
                            }
                            break;
                        }
                    case "kestrel":
                        {
                            using (var host = WebHostBuilderHelper.GetWebHost(options.Protocol, options.Port, ServerType.Kestrel))
                            {
                                host.Run();
                            }
                            break;
                        }
                    default:
                        break;
                }

                return 0;
            },
            errors =>
            {
                return 1;
            });
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal sealed class Options
    {
        [Option(Default = "weblistener", HelpText = "Host - Options [weblistener] or [kestrel] or [servicefabric-weblistener] or [servicefabric-kestrel]")]
        public string Host { get; set; }

        [Option(Default = "http", HelpText = "Protocol - Options [http] or [https]")]
        public string Protocol { get; set; }

        [Option(Default = "localhost", HelpText = "IP Address or Uri - Example [localhost] or [127.0.0.1]")]
        public string IpAddressOrFQDN { get; set; }

        [Option(Default = "5000", HelpText = "Port - Example [80] or [5000]")]
        public string Port { get; set; }
    }
}
