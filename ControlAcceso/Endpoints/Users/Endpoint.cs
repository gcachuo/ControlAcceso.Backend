using Microsoft.AspNetCore.Mvc;

namespace ControlAcceso.Endpoints.Users
{
    [ApiController]
    [Route("users")]
    public class Endpoint : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult RegisterUser()
        {
            return Ok(new Response { Message = "OK" });
        }
        
        [HttpPatch("{idUser}")]
        public IActionResult EditUser(string idUser)
        {
            return Ok(new Response { Message = "OK" });
        }
    }
}