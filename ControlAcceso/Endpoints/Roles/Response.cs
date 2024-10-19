using ControlAcceso.Data.Model;

namespace ControlAcceso.Endpoints.Roles
{
    public class Response:IResponse
    {
        public string? Message { get; set; }
        public IEnumerable<RoleModel>? Roles { get; set; }
        
    }
}