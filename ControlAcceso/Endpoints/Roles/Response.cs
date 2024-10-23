using ControlAcceso.Data.Model;

namespace ControlAcceso.Endpoints.Roles
{
    public class Response:IResponse
    {
        public string? Message { get; set; }
        public List<RoleModel>? Roles { get; set; }
        
    }
}