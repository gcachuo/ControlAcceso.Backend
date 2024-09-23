using ControlAcceso.Data.Model;
using ControlAcceso.Services.DBService;

namespace ControlAcceso.Data
{
    public class UsersDbContext
    {
        private IDbService DbService { get; set; }

        public UsersDbContext(IDbService dbService)
        {
            DbService = dbService;
        }

        public void InsertUser(UserModel user)
        {
            DbService.Insert("""
                                INSERT INTO Users(username, email, firstname, second_name, lastname, second_lastname, password, phone_number, address)
                                VALUES (@username, @email, @firstname, @second_name, @lastname, @second_lastname, @password, @phone_number, @address)
                             """,
                new()
                {
                    { "@username", user.Username}, 
                    { "@email", user.Email}, 
                    { "@firstname", user.FirstName}, 
                    { "@second_name", user.SecondName}, 
                    { "@lastname", user.Lastname}, 
                    { "@second_lastname", user.SecondLastname}, 
                    { "@password", user.Password}, 
                    { "@phone_number", user.PhoneNumber}, 
                    { "@address", user.Address}, 
                }
            );
        }
    }
}