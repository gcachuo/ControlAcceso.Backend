using System.Data;
using ControlAcceso.Data.Roles;
using Microsoft.AspNetCore.Mvc;
using ControlAcceso.Data.Model;

namespace ControlAcceso.Endpoints.Roles
{
    [ApiController]
    [Route("roles")]
    public class Endpoint : ControllerBase
    {
        private IRolesDbContext? _roles { get; }
        
        public Endpoint(IRolesDbContext? roles)
        {
            _roles = roles;
        }

        [HttpGet]
        public IActionResult GetRoleList()
        {
            var roles = _roles?.SelectRole();
            return Ok(new RoleResponse {Message = "OK", Roles=roles});
            
        }

        [HttpPost("create")]
        public IActionResult CreateRole([FromBody] Request request)
        {
            try
            {
                _roles?.InsertRole(new(){Name = request.Name,});
                return Ok(new Response { Message = "OK" });
            }
            catch (DataException e)
            {
                return BadRequest(new Response { Message = e.Message });
            }
        }

        [HttpPatch("{id:int}")]
        public IActionResult EditRole([FromBody] Request role)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(role.Name))
                {
                    return BadRequest(new { Message = "El campo 'name' es obligatorio." });
                }

                var existingRole = _roles?.SelectRole();
                if (existingRole == null)
                {
                    return NotFound(new { Message = "El rol no existe error endpoint." });
                }

                var roleModel = new RoleModel
                {
                    Name = role.Name
                };

                _roles?.UpdateRoleName(roleModel);

                return Ok(new Response { Message = "Rol actualizado exitosamente." });
            }
            catch (DataException e)
            {
                return BadRequest(new Response { Message = e.Message });
            }
        }

    }
}