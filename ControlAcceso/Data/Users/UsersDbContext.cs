using System.Data;
using ControlAcceso.Data.Model;
using ControlAcceso.Services.DBService;
using Npgsql;

namespace ControlAcceso.Data.Users
{
    public class UsersDbContext : IUsersDbContext
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
                DbService.ExecuteNonQuery("""
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

        public void UpdateUser(UserModel user, int idUser)
        {
            try
            {
                var insertQuery = @"
                    UPDATE Users
                    SET email = @Email,
                        firstname = @FirstName,
                        second_name = @SecondName,
                        lastname = @LastName,
                        second_lastname = @SecondLastname,
                        password = @Password,
                        phone_number = @PhoneNumber,
                        address = @Address
                    WHERE id = @IdUser";

                DbService.ExecuteNonQuery(insertQuery, new()
                {
                    { "@IdUser", idUser },
                    { "@Email", user.Email },
                    { "@FirstName", user.FirstName },
                    { "@SecondName", user.SecondName },
                    { "@LastName", user.Lastname },
                    { "@SecondLastname", user.SecondLastname },
                    { "@Password", user.Password },
                    { "@PhoneNumber", user.PhoneNumber },
                    { "@Address", user.Address }
                });
            }   
            catch (PostgresException e)
            {
            
                throw;
            }
        }

        public UserModel? SelectUser(int id)
        {
            var row = DbService.ExecuteReader("SELECT * FROM Users where id=@id", new() { { "@id", id } }).SingleOrDefault();
            if (row == null)
                return null;
            return new()
            {
                Address = row["address"]?.ToString(),
                PhoneNumber = row["phone_number"]?.ToString(),
                Username = row["username"]?.ToString(),
                Email = row["email"]?.ToString(),
                FirstName = row["firstname"]?.ToString(),
                SecondName = row["second_name"]?.ToString(),
                Lastname = row["lastname"]?.ToString(),
                SecondLastname = row["second_lastname"]?.ToString(),
            };
        }

        public string? SelectPassword(string? username)
        {
           var row=DbService.ExecuteReader("SELECT password FROM Users where username=@username or email=@username or phone_number=@username", 
               new() { { "@username", username } }).SingleOrDefault();
           return row?["password"].ToString();
        }
    }
}