using System.Data;
using ControlAcceso.Data.Roles;
using ControlAcceso.Data.Permissions;
using Microsoft.AspNetCore.Mvc;
using ControlAcceso.Data.Model;

namespace ControlAcceso.Endpoints.Roles
{
    [ApiController]
    [Route("roles")]
    public class Endpoint : ControllerBase
    {
        private IRolesDbContext? _roles { get; }
        private IPermissionsDbContext? _permissions { get; }

        public Endpoint(IRolesDbContext? roles, IPermissionsDbContext? permissions)
        {
            _roles = roles;
            _permissions = permissions;
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
        public IActionResult EditRole(int id, [FromBody] Request role)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(role.Name))
                {
                    return BadRequest(new Response{ Message = "El campo 'name' es obligatorio" });
                }
                
                _roles?.UpdateRoleName(id, new RoleModel { Id = id, Name = role.Name });

                return Ok(new Response { Message = "Rol actualizado exitosamente." });
            }
            catch (DataException e)
            {
                return BadRequest(new Response { Message = e.Message });
            }
        }
        [HttpGet("{idRole}/nodes/{idUser}")]
        public IActionResult GetPermissions(int idRole, int idUser)
        {
            var groupedPermissions = _permissions?.GetGroupedPermissions(idRole, idUser);

                if (groupedPermissions == null || !groupedPermissions.Any())
                {
                    return NotFound(new PermissionsResponse { Message = "No se encontraron permisos.", Permissions = new Dictionary<string, List<string>>() });
                }

                return Ok(new PermissionsResponse { Message = "OK", Permissions = groupedPermissions });         
        }
    }
}