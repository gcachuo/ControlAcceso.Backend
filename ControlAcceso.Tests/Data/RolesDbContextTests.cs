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
            

    
        [Fact]
        public void SelectRole_Returns_List_Of_Roles()
        {
            // Arrange
            var mockDbService = new Mock<IDbService>();


            var fakeRows = new List<Dictionary<string, dynamic>>()
            {
                new Dictionary<string, dynamic> { { "name", "Admin" }, { "id", "1" } },
                new Dictionary<string, dynamic> { { "name", "User" }, { "id", "2" } }
            };

           
            mockDbService.Setup(db => db.ExecuteReader("SELECT * FROM Roles", It.IsAny<Dictionary<string, dynamic>>()))
                        .Returns(fakeRows);

            var dbContext = new RolesDbContext(mockDbService.Object);

            // Act
            var result = dbContext.SelectRole();

            // Assert
            Assert.NotNull(result); 
            Assert.Equal(2, result.Count()); 
            Assert.Equal("Admin", result.First().Name); 
            Assert.Equal("1", result.First().Id); 
        }

        [Fact]
        public void SelectRole_Returns_EmptyList()
        {
            // Arrange
            var mockDbService = new Mock<IDbService>();

            
            mockDbService.Setup(db => db.ExecuteReader("SELECT * FROM Roles", It.IsAny<Dictionary<string, dynamic>>()))
                        .Returns(new List<Dictionary<string, dynamic>>());

            var dbContext = new RolesDbContext(mockDbService.Object);

            // Act
            var result = dbContext.SelectRole();

            // Assert
            Assert.NotNull(result); 
            Assert.Empty(result);
        }
    }
}