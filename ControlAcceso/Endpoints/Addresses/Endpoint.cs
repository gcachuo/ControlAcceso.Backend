using ControlAcceso.Data.Addresses;
using Microsoft.AspNetCore.Mvc;

namespace ControlAcceso.Endpoints.Addresses
{
    [ApiController]
    [Route("Addresses")]

    public class Endpoint : ControllerBase
    {
        private IAddressesDbContext? _addresses { get;}

        public Endpoint(IAddressesDbContext addresses)
        {
            _addresses = addresses;
        }

        [HttpPost]
         public IActionResult GetAddress()
        {
            var addresses=_addresses?.SelectAddress();
            return Ok(new Response { Message = "OK", Addresses=addresses });
        }
    }
    
}