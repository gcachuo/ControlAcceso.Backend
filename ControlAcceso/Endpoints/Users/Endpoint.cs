using System.Data;
using ControlAcceso.Data.Users;
using ControlAcceso.Tools;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult EditUser(string idUser,[FromBody] Request request)
        {
            try
            {
                var updateQuery = @"
                    UPDATE Users
                    SET email = @Email,
                        firstname = @FirstName,
                        second_name = @SecondName,
                        lastname = @LastName,
                        second_lastname = @SecondLastname,
                        password = @Password,
                        phone_number = @PhoneNumber,
                        address = @Address
                    WHERE idUser = @IdUser";

                var hashedPassword = PasswordHasher.HashPassword(request.Password);

                var updateParameters = new Dictionary<string, dynamic>
                {
                    { "@IdUser", idUser },
                    { "@Email", request.Email },
                    { "@FirstName", request.FirstName },
                    { "@SecondName", request.SecondName },
                    { "@LastName", request.FirstSurname },
                    { "@SecondLastname", request.SecondSurname },
                    { "@Password", hashedPassword },
                    { "@PhoneNumber", request.Phone },
                    { "@Address", request.Address }
                };

                _users?.UpdateUser(updateQuery, updateParameters);

                return Ok(new Response { Message = "Usuario actualizado correctamente" });
            }
            catch (DataException e)
            {
                return BadRequest(new Response { Message = e.Message });
            }
            
        }
    }
}