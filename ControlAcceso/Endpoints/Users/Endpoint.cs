using System.Data;
using ControlAcceso.Data.Users;
using ControlAcceso.Tools;
using Microsoft.AspNetCore.Mvc;
using ControlAcceso.Data.Model;

namespace ControlAcceso.Endpoints.Users
{
    [ApiController]
    [Route("users")]
    public class Endpoint : ControllerBase
    {
        private IUsersDbContext? _users { get; }
        
        public Endpoint(IUsersDbContext? users)
        {
            _users = users;
        }
        
        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] Request request)
        {
            var hashedPassword=PasswordHasher.HashPassword(request.Password);
            var username = $"{request.FirstName?.ToLower().Replace(" ","")}.{request.FirstSurname?.ToLower().Replace(" ","")}";
            try
            {
                _users?.InsertUser(new()
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
            catch (DataException e)
            {
                return BadRequest(new Response { Message = e.Message });
            }
        }


        
        [HttpPatch("{idUser}")]
        public IActionResult EditUser(int idUser, [FromBody] Request request)
        {
            try
            {
                
                var hashedPassword = PasswordHasher.HashPassword(request.Password);
                
                var user = new UserModel
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    SecondName = request.SecondName,
                    Lastname = request.FirstSurname,
                    SecondLastname = request.SecondSurname,
                    Password = hashedPassword,
                    PhoneNumber = request.Phone,
                    Address = request.Address
                };

                
                _users?.UpdateUser(user, idUser);

                return Ok(new Response { Message = "Usuario actualizado correctamente" });
                }
            catch (DataException e)
            {
                return BadRequest(new Response { Message = e.Message });
            }
        }

        [HttpGet("{idUser}")]
        public IActionResult GetUser(int idUser)
        {
            var user=_users?.SelectUser(idUser);
            return Ok(new Response { Message = "OK", User=user });
        }
    }
}