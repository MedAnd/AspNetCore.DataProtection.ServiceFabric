using AspNetCore.DataProtection.ServiceFabric.Interfaces;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AspNetCore.DataProtection.ServiceFabric.Repositories
{
    public class ServiceFabricXmlRepository : IXmlRepository
    {
        private readonly string _serviceUri;

        public ServiceFabricXmlRepository(string serviceUri)
        {
            _serviceUri = serviceUri;
        }

        public IReadOnlyCollection<XElement> GetAllElements()
        {
            var proxy = ServiceProxy.Create<IDataProtectionService>(new Uri(_serviceUri), new ServicePartitionKey());
            var elements = Task.Run(() => proxy.GetAllDataProtectionElements()).GetAwaiter().GetResult();
            return elements.AsReadOnly();
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            var proxy = ServiceProxy.Create<IDataProtectionService>(new Uri(_serviceUri), new ServicePartitionKey());
            Task.Run(() => proxy.AddDataProtectionElement(element)).GetAwaiter().GetResult();
        }
    }
}
