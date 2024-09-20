using Microsoft.AspNetCore.Mvc;

namespace ControlAcceso.Endpoints.Roles
{
    [ApiController]
    [Route("roles")]
    public class Endpoint : ControllerBase
    {
        [HttpPost("create")]
        public IActionResult CreateRole()
        {
            return Ok(new Response { Message = "OK" });
        }
    }
}