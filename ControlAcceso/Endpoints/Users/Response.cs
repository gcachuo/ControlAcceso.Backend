using ControlAcceso.Data.Model;

namespace ControlAcceso.Endpoints.Users
{
    public class UserResponse:IResponse
    {
        public string? Message { get; set; }
        public UserModel? User { get; set; }

        public List<UserModel>? Users { get; set; }

    }

    public class LoginResponse : IResponse
    {
        public string? Message { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}