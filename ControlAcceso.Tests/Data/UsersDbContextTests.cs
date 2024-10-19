using ControlAcceso.Data.Model;
using ControlAcceso.Data.Users;
using ControlAcceso.Services.DBService;
using Moq;

namespace ControlAcceso.Tests.Data
{
    public class UsersDbContextTests
    {
        private Mock<IDbService> _dbServiceMock = new(MockBehavior.Default);

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

        [Fact]
        public void Should_Select_User()
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
                    {"role_id","1"},
                }, });
            
            //Act
            var context = new UsersDbContext(_dbServiceMock.Object);
            context.SelectUser(1);

            //Assert
            _dbServiceMock.Verify(x=>x.ExecuteReader(It.IsAny<string>(),It.IsAny<Dictionary<string,dynamic>>()));
        }
    }
}