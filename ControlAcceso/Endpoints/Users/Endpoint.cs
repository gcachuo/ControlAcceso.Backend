using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using ControlAcceso.Data.Users;
using ControlAcceso.Tools;
using Microsoft.AspNetCore.Mvc;
using ControlAcceso.Data.Model;
using ControlAcceso.Data.RefreshTokens;
using ControlAcceso.Tools.HttpContext;
using Microsoft.IdentityModel.Tokens;

namespace ControlAcceso.Endpoints.Users
{
    [ApiController]
    [Route("users")]
    public class Endpoint : ControllerBase
    {
        private IUsersDbContext? _users { get; }
        private IRefreshTokensDbContext? _refreshTokens { get; }
        private IHttpContext? _httpContext { get; }
        
        public Endpoint(IUsersDbContext? users, IRefreshTokensDbContext? refreshTokens, IHttpContext? httpContext)
        {
            _users = users;
            _refreshTokens = refreshTokens;
            _httpContext = httpContext;
        }
        
        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] UserRequest request)
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
                return Ok(new UserResponse { Message = "OK" });
            }
            catch (DataException e)
            {
                return BadRequest(new UserResponse { Message = e.Message });
            }
        }


        
        [HttpPatch("{idUser}")]
        public IActionResult EditUser(int idUser, [FromBody] UserRequest request)
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

                return Ok(new UserResponse { Message = "Usuario actualizado correctamente" });
                }
            catch (DataException e)
            {
                return BadRequest(new UserResponse { Message = e.Message });
            }
        }

        [HttpGet("{idUser}")]
        public IActionResult GetUser(int idUser)
        {
            var user=_users?.SelectUser(idUser);
            return Ok(new UserResponse { Message = "OK", User=user });
        }

        [HttpPost("login")]
        public IActionResult LoginUser([FromBody] LoginRequest request)
        {
           var passwordHash = _users.SelectPassword(request.Username);
           if (passwordHash is null || !PasswordHasher.VerifyPassword(request.Password, passwordHash))
               return Unauthorized(new LoginResponse { AccessToken = "", RefreshToken = "", Message = "Unauthorized" });
           
           var user = _users.SelectUser(request.Username);

           var claims = new List<Claim>
           {
               new("UserId", user.Id.ToString()),
               new("Role", user.Role)
           };
           var signingKey = Environment.GetEnvironmentVariable("JWT_SIGNING_KEY");
           var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
           var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

           var accessToken = GenerateAccessToken(claims,signingKey,issuer,audience);
           var refreshToken = GenerateRefreshToken();
           
           var ipAddress = _httpContext.GetIpAddress();
           _refreshTokens.InsertToken(refreshToken, (int)user.Id!, ipAddress, request.UserAgent);
           
           return Ok(new LoginResponse { AccessToken = accessToken, RefreshToken = refreshToken, Message = "OK" });
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims, string signingKey, string issuer, string audience)
        {
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(signingKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.Now.AddHours(1),  // Duración del Access Token (1 hora)
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}