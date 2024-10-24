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
                                    INSERT INTO Users(username, email, firstname, second_name, lastname, second_lastname, password, phone_number, address, role_id)
                                    VALUES (@username, @email, @firstname, @second_name, @lastname, @second_lastname, @password, @phone_number, @address, @role_id)
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
                        { "@role_id", user.RoleId },
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
                        phone_number = @PhoneNumber,
                        address = @Address,
                        role_id = @RoleId
                    WHERE id = @IdUser";

                DbService.ExecuteNonQuery(insertQuery, new()
                {
                    { "@IdUser", idUser },
                    { "@Email", user.Email },
                    { "@FirstName", user.FirstName },
                    { "@SecondName", user.SecondName },
                    { "@LastName", user.Lastname },
                    { "@SecondLastname", user.SecondLastname },
                    { "@PhoneNumber", user.PhoneNumber },
                    { "@Address", user.Address },
                    { "@RoleId", user.RoleId }
                });
            }
            finally
            {
                
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
                RoleId = row["role_id"]?.ToString(),
            };
        }

        
        public UserModel? SelectUser(string username)
        {
            var row = DbService.ExecuteReader("SELECT u.*,r.name role FROM Users u left join roles r on r.id=u.role_id where (username=@username or email=@username or phone_number=@username) AND enable = 1", new() { { "@username", username } }).SingleOrDefault();
            if (row == null)
                return null;
            return new()
            {
                Id = row["id"] as int?,
                Role = row["role"].ToString(),
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

        public List<UserModel> SelectUserList()
        {
            var rows = DbService.ExecuteReader("SELECT * FROM Users WHERE enable = 1", new Dictionary<string, dynamic>());
            if (rows == null)
            {
                throw new DataException("No se encontraron usuarios activos."); 
            }

            var users = new List<UserModel>();

            foreach (var row in rows)
            {
                if (row == null)
                {
                    continue; 
                }

                users.Add(new UserModel
                {
                    Address = row["address"]?.ToString(),
                    PhoneNumber = row["phone_number"]?.ToString(),
                    FirstName = row["firstname"]?.ToString(),
                    SecondName = row["second_name"]?.ToString(),
                    Lastname = row["lastname"]?.ToString(),
                    SecondLastname = row["second_lastname"]?.ToString(),
                });
            }

            return users;
        }

        public void DisableUser(int idUser)
        {
            var updateQuery = "UPDATE Users SET enable = 0 WHERE id = @IdUser";
            DbService.ExecuteNonQuery(updateQuery, new() { { "@IdUser", idUser } });
        }

    }
}