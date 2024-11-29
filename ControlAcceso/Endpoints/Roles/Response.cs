using ControlAcceso.Data.Model;

namespace ControlAcceso.Endpoints.Roles
{
    public class RoleResponse:IResponse
    {
        public string? Message { get; set; }
        public List<RoleModel>? Roles { get; set; }
        
    }

    public class Response:IResponse
    {
        public string? Message {get; set;}
        
    }
}