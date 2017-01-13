using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Collections.Generic;
using System.Fabric;

namespace ServiceFabric.DataProtection.Web
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class WebService : StatelessService
    {
        /// <summary>
        /// 
        /// </summary>
        ServerType _serverType;

        public WebService(StatelessServiceContext context, ServerType serverType)
            : base(context)
        {
            _serverType = serverType;
        }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(serviceContext =>
                {
                    switch (_serverType)
                    {
                        case ServerType.WebListener :
                            {
                                return new WebListenerCommunicationListener(serviceContext, "ServiceEndpoint", url =>
                                {
                                    return WebHostBuilderHelper.GetServiceFabricWebHost(_serverType);
                                });
                            }
                        case ServerType.Kestrel:
                            {
                                return new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", url =>
                                {
                                    return WebHostBuilderHelper.GetServiceFabricWebHost(_serverType);
                                });
                            }
                        default:
                            return null;
                    }
                })                  
            };
        }
    }
}