using System.Text.Json.Serialization;
using ControlAcceso.Data.Model;

namespace ControlAcceso.Endpoints.Users
{
    public class UserResponse:IResponse
    {
        public string? Message { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public UserModel? User { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<UserModel>? Users { get; set; }

    }

    public class LoginResponse : IResponse
    {
        public string? Message { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? AccessToken { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? RefreshToken { get; set; }
    }

    public class UserDelete:IResponse
    {
        public string? Message { get; set;}
    }
}