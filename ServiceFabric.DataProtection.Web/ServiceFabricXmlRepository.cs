using AspNetCore.DataProtection.ServiceFabric;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ServiceFabric.DataProtection.Web
{
    public class ServiceFabricXmlRepository : IXmlRepository
    {
        public IReadOnlyCollection<XElement> GetAllElements()
        {
            var proxy = ServiceProxy.Create<IDataProtectionService>(new Uri("fabric:/ServiceFabric.DataProtection/DataProtectionService"), new ServicePartitionKey());
            return proxy.GetAllDataProtectionElements().Result.AsReadOnly();
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            var proxy = ServiceProxy.Create<IDataProtectionService>(new Uri("fabric:/ServiceFabric.DataProtection/DataProtectionService"), new ServicePartitionKey());
            proxy.AddDataProtectionElement(element).Wait();
        }
    }
}
