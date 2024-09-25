using ControlAcceso.Data.Model;

namespace ControlAcceso.Data.Users
{
    public interface IUsersDbContext
    {
        public void InsertUser(UserModel user);
    }
}