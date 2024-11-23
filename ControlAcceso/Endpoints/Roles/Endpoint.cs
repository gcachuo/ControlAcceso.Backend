using System.Data;
using ControlAcceso.Data.Roles;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(new Response {Message = "OK", Roles=roles});
            
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

        [HttpPatch("{id}")]
        public IActionResult EditRole(int id, [FromBody] RoleModel role)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(role.Name))
                {
                    return BadRequest(new { Message = "El campo 'name' es obligatorio." });
                }

                var existingRole = _roles?.SelectRoleById(id);
                if (existingRole == null)
                {
                    return NotFound(new { Message = $"El rol con ID {id} no existe." });
                }

                // Actualizar el nombre del rol
                _roles?.UpdateRoleName(id, role);

                return Ok(new { Message = "Rol actualizado exitosamente." });
            }
            catch (DataException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocurrió un error inesperado.", Error = ex.Message });
            }
        }

    }
}