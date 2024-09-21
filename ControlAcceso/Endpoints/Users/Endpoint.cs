using System.Net;
using ControlAcceso.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControlAcceso.Endpoints.Users
{
    [ApiController]
    [Route("users")]
    public class Endpoint : ControllerBase
    {

        private readonly string fileUsers = Directory.GetCurrentDirectory() + "/Data/Users.json";

        private readonly IFileService _fileService;

        public EndPoint(IFileService fileService)
        {
            _fileService = fileService;
        }
        private List<Users

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