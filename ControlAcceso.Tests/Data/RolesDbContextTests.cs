using ControlAcceso.Data.Model;
using ControlAcceso.Data.Roles;
using ControlAcceso.Data.Users;
using ControlAcceso.Services.DBService;
using Moq;

namespace ControlAcceso.Tests.Data
{
    public class RolesDbContextTests
    {
        private readonly Mock<IDbService> _dbServiceMock = new(MockBehavior.Default);

        [Fact]
        public void Should_Insert_Role_Successfully()
        {
            //Arrange
            //Mock
            //Act
            var context = new RolesDbContext(_dbServiceMock.Object);
            context.InsertRole(new());

            //Assert
            _dbServiceMock.Verify(x=>x.ExecuteNonQuery(It.IsAny<string>(),It.IsAny<Dictionary<string,dynamic>>()));
        }
    }
}