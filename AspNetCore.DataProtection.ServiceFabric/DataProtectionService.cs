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
        public DataProtectionService(StatefulServiceContext context, IReliableStateManager stateManager) : base(context, stateManager as IReliableStateManagerReplica)
        {

        }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener(context => this.CreateServiceRemotingListener(context))
            };
        }

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
