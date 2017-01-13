using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AspNetCore.DataProtection.ServiceFabric
{
    internal sealed class DataProtectionService : StatefulService, IDataProtectionService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="stateManager"></param>
        public DataProtectionService(StatefulServiceContext context, IReliableStateManager stateManager)
            : base(context, stateManager as IReliableStateManagerReplica)
        {

        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see http://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener(context => this.CreateServiceRemotingListener(context))
            };
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override Task RunAsync(CancellationToken cancellationToken)
        {
            //No Running code
            return Task.FromResult(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<XElement>> GetAllDataProtectionElements()
        {
            var elements = new List<XElement>();

            var dictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, XElement>>("AspNetCore.DataProtection");
            using (var tx = this.StateManager.CreateTransaction())
            {
                var enumerable = await dictionary.CreateEnumerableAsync(tx);
                var enumerator = enumerable.GetAsyncEnumerator();
                var token = new CancellationToken();

                while (await enumerator.MoveNextAsync(token))
                {
                    elements.Add(enumerator.Current.Value);
                }
            }

            return elements;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public async Task<XElement> AddDataProtectionElement(XElement element)
        {
            Guid id = Guid.Parse(element.Attribute("id").Value);

            var dictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, XElement>>("AspNetCore.DataProtection");
            using (var tx = this.StateManager.CreateTransaction())
            {
                var result = await dictionary.GetOrAddAsync(tx, id, element);
                await tx.CommitAsync();

                return result;
            }
        }
    }
}
