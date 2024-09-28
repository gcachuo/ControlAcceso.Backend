using ControlAcceso.Data.Model;

namespace ControlAcceso.Endpoints.Users
{
    public class Response:IResponse
    {
        public string? Message { get; set; }
        public UserModel? User { get; set; }
    }
}