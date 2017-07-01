using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace ServiceFabric.DataProtection.Web.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DataProtectionController : Controller
    {
        [HttpGet]
        [Route(nameof(Protect) + "/{value}")]
        [Produces(typeof(string))]
        public string Protect(string value)
        {
            var protector = HttpContext.RequestServices.GetDataProtector("servicefabric.dataprotection-purpose");
            var protectedData = protector.Protect(value);

            return $"Protect returned: {protectedData}";
        }

        [HttpGet]
        [Route(nameof(Unprotect) + "/{value}")]
        [Produces(typeof(string))]
        public string Unprotect(string value)
        {
            var protector = HttpContext.RequestServices.GetDataProtector("servicefabric.dataprotection-purpose");
            var protectedData = protector.Unprotect(value);

            return $"Unprotect returned: {protectedData}";
        }
    }
}
