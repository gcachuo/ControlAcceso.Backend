using ControlAcceso.Data.Model;
using ControlAcceso.Data.Users;
using ControlAcceso.Services.DBService;
using Moq;

namespace ControlAcceso.Tests.Data
{
    public class UsersDbContextTests
    {
        private Mock<IDbService> _dbServiceMock = new(MockBehavior.Default);
        private Mock<IDbService> _mockDbService;
        private UsersDbContext _usersDbContext;

        public UsersDbContextTests()
        {
        
            _mockDbService = new Mock<IDbService>();
            _usersDbContext = new UsersDbContext(_mockDbService.Object);
        }

        [Fact]
        public void Should_Insert_User()
        {
            //Arrange
            //Mock
            //Act
            var context = new UsersDbContext(_dbServiceMock.Object);
            context.InsertUser(new());

            //Assert
            _dbServiceMock.Verify(x=>x.ExecuteNonQuery(It.IsAny<string>(),It.IsAny<Dictionary<string,dynamic>>()));
        }

        [Fact] 
        public void Should_Edit_User_Successfully()
        {
            //Arrange
            var idUser = 1;
            var user = new UserModel();

            //Mock

            //Act
            var context = new UsersDbContext(_dbServiceMock.Object);
            context.UpdateUser(user, idUser);

            //Assert
           _dbServiceMock.Verify(x=>x.ExecuteNonQuery(It.IsAny<string>(),It.IsAny<Dictionary<string,dynamic>>()));
        }

        [Theory]
        [InlineData(1)]
        [InlineData("username")]
        public void Should_Select_User(dynamic username)
        {
            //Arrange
            //Mock
            _dbServiceMock.Setup(x => x.ExecuteReader(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()))
                .Returns(new List<Dictionary<string, object>>() { new()
                {
                    { "id", "1" },
                    {"address",""},
                    {"phone_number",""},
                    {"username",""},
                    {"email",""},
                    {"firstname",""},
                    {"second_name",""},
                    {"lastname",""},
                    {"second_lastname",""},
                    {"role","Administrador"},
                    {"role_id","1"},
                }, });
            
            //Act
            var context = new UsersDbContext(_dbServiceMock.Object);
            context.SelectUser(username);

            //Assert
            _dbServiceMock.Verify(x=>x.ExecuteReader(It.IsAny<string>(),It.IsAny<Dictionary<string,dynamic>>()));
        }

        [Fact]
        public void SelectUserList_Returns_List_Of_Users()
        {
            // Arrange
            var mockDbService = new Mock<IDbService>();

            var fakeRows = new List<Dictionary<string, dynamic>>()
            {
                new Dictionary<string, dynamic>
                {
                    { "address", "Calle 1" },
                    { "phone_number", "4771234567" },
                    { "firstname", "Juan" },
                    { "second_name", "Jesus" },
                    { "lastname", "Perez" },
                    { "second_lastname", "Perez" }
                },
                new Dictionary<string, dynamic>
                {
                    { "address", "Calle 2" },
                    { "phone_number", "4771234567" },
                    { "firstname", "Victor" },
                    { "second_name", "Andres" },
                    { "lastname", "Ornelas" },
                    { "second_lastname", "Cervantes" }
                }
            };

            mockDbService.Setup(db => db.ExecuteReader("SELECT * FROM Users WHERE enable = 1", It.IsAny<Dictionary<string, dynamic>>()))
                        .Returns(fakeRows);

            var dbContext = new UsersDbContext(mockDbService.Object);

            // Act
            var result = dbContext.SelectUserList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            var expectedUsers = new List<UserModel>
            {
                new UserModel
                {
                    Address = "Calle 1",
                    PhoneNumber = "4771234567",
                    FirstName = "Juan",
                    SecondName = "Jesus",
                    Lastname = "Perez",
                    SecondLastname = "Perez"
                },
                new UserModel
                {
                    Address = "Calle 2",
                    PhoneNumber = "4771234567",
                    FirstName = "Victor",
                    SecondName = "Andres",
                    Lastname = "Ornelas",
                    SecondLastname = "Cervantes"
                }
            };

            int index = 0; 
            foreach (var actualUser in result)
            {
                Assert.Equal(expectedUsers[index].Address, actualUser.Address);
                Assert.Equal(expectedUsers[index].PhoneNumber, actualUser.PhoneNumber);
                Assert.Equal(expectedUsers[index].FirstName, actualUser.FirstName);
                Assert.Equal(expectedUsers[index].SecondName, actualUser.SecondName);
                Assert.Equal(expectedUsers[index].Lastname, actualUser.Lastname);
                Assert.Equal(expectedUsers[index].SecondLastname, actualUser.SecondLastname);
                index++;
            }
        }


         [Fact]
        public void SelectPassword_ReturnsPassword_WhenUserExists()
        {
            // Arrange
            var mockDbService = new Mock<IDbService>();
            var fakeRow = new Dictionary<string, dynamic> { { "password", "password123" } };

            mockDbService.Setup(db => db.ExecuteReader(
                "SELECT password FROM Users where username=@username or email=@username or phone_number=@username",
                It.IsAny<Dictionary<string, dynamic>>()))
                .Returns(new List<Dictionary<string, dynamic>> { fakeRow });

            var dbContext = new UsersDbContext(mockDbService.Object);
            string username = "user1";

            // Act
            var result = dbContext.SelectPassword(username);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("password123", result);
        }

         [Fact]
        public void DisableUser_When_User_Is_Disabled()
        {
            // Arrange
            int userId = 1;

            // Act
            _usersDbContext.DisableUser(userId);

            // Assert
            _mockDbService.Verify(db => db.ExecuteNonQuery(
                "UPDATE Users SET enable = 0 WHERE id = @IdUser",
                It.Is<Dictionary<string, object>>(d => d["@IdUser"].Equals(userId))
            ), Times.Once);
        }

    }
}