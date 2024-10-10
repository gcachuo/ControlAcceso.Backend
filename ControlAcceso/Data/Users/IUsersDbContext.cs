using ControlAcceso.Data.Model;

namespace ControlAcceso.Data.Users
{
    public interface IUsersDbContext
    {
        public void UpdateUser(UserModel user, int idUser); 

        public void InsertUser(UserModel user);
        public UserModel? SelectUser(int id);
        public UserModel? SelectUser(string username);
        public string? SelectPassword(string? username);
    }
}