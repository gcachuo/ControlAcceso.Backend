using ControlAcceso.Data.Model;

namespace ControlAcceso.Data.Users
{
    public interface IUsersDbContext
    {
        public void UpdateUser(UserModel user, string idUser); 

        public void InsertUser(UserModel user);
        public UserModel SelectUser(int id);
    }
}