using Microsoft.ServiceFabric.Services.Remoting;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AspNetCore.DataProtection.ServiceFabric
{
    public interface IDataProtectionService : IService
    {
        Task<XElement> AddDataProtectionElement(XElement element);
        Task<List<XElement>> GetAllDataProtectionElements();
    }
}