using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace ControlAcceso.Endpoints.Addresses
{
    [ApiController]
    [Route("addresses")]
    public class Endpoint : ControllerBase
    {
        [HttpPost("create")]
        public IActionResult CreateAddress([FromBody] Request request)
        {
            try
            {
                return Ok(new Response { Message = "OK" });
            }
            catch (DataException e)
            {
                return BadRequest(new Response { Message = e.Message });
            }
        }
    }
}