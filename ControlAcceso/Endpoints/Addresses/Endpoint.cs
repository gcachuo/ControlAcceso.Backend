using System.Data;
using ControlAcceso.Data.Addresses;
using Microsoft.AspNetCore.Mvc;

namespace ControlAcceso.Endpoints.Addresses
{
    [ApiController]
    [Route("addresses")]
    public class Endpoint : ControllerBase
    {
        private IAddressesDbContext? _addresses { get; }
        public Endpoint(IAddressesDbContext? addresses)
        {
            _addresses = addresses;
        }

        [HttpGet]
         public IActionResult GetAddress()
        {
            var addresses=_addresses?.SelectAddress();
            return Ok(new Response { Message = "OK", Addresses=addresses });
        }

        [HttpPost("create")]
        public IActionResult CreateAddress([FromBody] Request request)
        {
            try
            {
                _addresses?.InsertAddress(new()
                {
                    Street = request.Street,
                    Number = request.Number,
                });
                return Ok(new Response { Message = "OK" });
            }
            catch (DataException e)
            {
                return BadRequest(new Response { Message = e.Message });
            }
        }
    }
}