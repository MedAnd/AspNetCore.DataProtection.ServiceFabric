using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;

[assembly: FabricTransportServiceRemotingProvider(RemotingListener = RemotingListener.V2Listener, RemotingClient = RemotingClient.V2Client)]

namespace AspNetCore.DataProtection.ServiceFabric.Interfaces
{
    public interface IDataProtectionService : IService
    {
        Task<XElement> AddDataProtectionElement(XElement element);
        Task<List<XElement>> GetAllDataProtectionElements();
    }
}