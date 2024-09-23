using ControlAcceso.Data.Model;
using ControlAcceso.Tools;
using Microsoft.AspNetCore.Mvc;

namespace ControlAcceso.Endpoints.Users
{
    [ApiController]
    [Route("users")]
    public class Endpoint : ControllerBase
    {
        private Data.UsersDbContext? Users { get; }
        
        public Endpoint(Data.UsersDbContext? users)
        {
            Users = users;
        }
        
        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] Request request)
        {
            var hashedPassword=PasswordHasher.HashPassword(request.Password);
            var username = $"{request.FirstName?.ToLower().Replace(" ","")}.{request.FirstSurname?.ToLower().Replace(" ","")}";
            Users?.InsertUser(new()
            {
                Username = username,
                Email = request.Email,
                FirstName = request.FirstName,
                SecondName = request.SecondName,
                Lastname = request.FirstSurname,
                SecondLastname = request.SecondSurname,
                Password = hashedPassword,
                PhoneNumber = request.Phone,
                Address = request.Address
            });
            return Ok(new Response { Message = "OK" });
        }

        [HttpPatch("{idUser}")]
        public IActionResult EditUser(string idUser)
        {
            return Ok(new Response { Message = "OK" });
        }
    }
}