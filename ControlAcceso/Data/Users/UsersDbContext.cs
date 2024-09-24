using System.Data;
using ControlAcceso.Data.Model;
using ControlAcceso.Services.DBService;
using Npgsql;

namespace ControlAcceso.Data.Users
{
    public class UsersDbContext:IUsersDbContext
    {
        private IDbService DbService { get; set; }

        public UsersDbContext(IDbService dbService)
        {
            DbService = dbService;
        }

        public void InsertUser(UserModel user)
        {
            try
            {
                DbService.Insert("""
                                    INSERT INTO Users(username, email, firstname, second_name, lastname, second_lastname, password, phone_number, address)
                                    VALUES (@username, @email, @firstname, @second_name, @lastname, @second_lastname, @password, @phone_number, @address)
                                 """,
                    new()
                    {
                        { "@username", user.Username },
                        { "@email", user.Email },
                        { "@firstname", user.FirstName },
                        { "@second_name", user.SecondName },
                        { "@lastname", user.Lastname },
                        { "@second_lastname", user.SecondLastname },
                        { "@password", user.Password },
                        { "@phone_number", user.PhoneNumber },
                        { "@address", user.Address },
                    }
                );
            }
            catch (PostgresException e)
            {
                if (e.Data["SqlState"]?.ToString() == "23505")
                {
                    throw new DataException("Usuario duplicado.");
                }
            }
        }
    }
}