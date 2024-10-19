using System;
using System.Collections.Generic;
using System.Data; // Asegúrate de que este espacio de nombres esté incluido
using ControlAcceso.Data.Model;
using ControlAcceso.Data.Roles;
using ControlAcceso.Services.DBService;
using Moq;
using Npgsql;
using Xunit;

namespace ControlAcceso.Tests.Data
{
    public class RolesDbContextTests
    {
        private readonly Mock<IDbService> _dbServiceMock = new(MockBehavior.Default);

        [Fact]
        public void Should_Insert_Role_Successfully()
        {
            // Arrange
            var context = new RolesDbContext(_dbServiceMock.Object);
            var role = new RoleModel { Name = "User" }; // Asegúrate de proporcionar un nombre para el rol.

            // Act
            context.InsertRole(role);

            // Assert
            _dbServiceMock.Verify(x => x.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()), Times.Once);
        }

        [Fact]
        public void InsertRole_ThrowsDataException_WhenRoleIsDuplicate()
        {
            // Arrange
            var mockDbService = new Mock<IDbService>();
            var dbContext = new RolesDbContext(mockDbService.Object);
            var duplicateRole = new RoleModel { Name = "Admin" };

            var postgresException = new PostgresException("Duplicated key", null, null, null, null)
            {
                Data = { ["SqlState"] = "23505" } 
            };

            mockDbService.Setup(db => db.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()))
                         .Throws(postgresException);

            // Act & Assert
            var exception = Assert.Throws<DataException>(() => dbContext.InsertRole(duplicateRole));
            Assert.Equal("Rol duplicado.", exception.Message);
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

            var expectedRoles = new List<(string Name, string Id)>
            {
                ("Admin", "1"),
                ("User", "2")
            };

            int index = 0; 
            foreach (var actualRole in result)
            {
                Assert.Equal(expectedRoles[index].Name, actualRole.Name);
                Assert.Equal(expectedRoles[index].Id, actualRole.Id);
                index++; 
            }
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
