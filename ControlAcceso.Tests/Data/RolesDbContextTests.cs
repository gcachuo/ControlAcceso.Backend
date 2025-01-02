using System.Data;
using ControlAcceso.Data.Model;
using ControlAcceso.Data.Roles;
using ControlAcceso.Services.DBService;
using FluentAssertions;
using Moq;
using Npgsql;

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
                new Dictionary<string, dynamic> { { "name", "Admin" }, { "id", 1 } },
                new Dictionary<string, dynamic> { { "name", "User" }, { "id", 2 } }
            };

            mockDbService.Setup(db => db.ExecuteReader("SELECT * FROM Roles", It.IsAny<Dictionary<string, dynamic>>()))
                        .Returns(fakeRows);

            var dbContext = new RolesDbContext(mockDbService.Object);

            // Act
            var result = dbContext.SelectRole();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            var expectedRoles = new List<(string Name, int Id)>
            {
                ("Admin", 1 ),
                ("User", 2 )
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

        [Fact]
        public void When_Duplicated_Role_Then_Throws_DataException()
        {
            //Arrange
            Mock<IDbService> dbServiceMock = new(MockBehavior.Strict);

            //Mock
            dbServiceMock.Setup(x => x.ExecuteNonQuery("""
                                                          INSERT INTO Roles(name)
                                                          VALUES (@name)
                                                       """, It.IsAny<Dictionary<string, object>>()))
                .Throws(new PostgresException("messageText","severity","invariantSeverity","23505"));
            
            //Act
            var context = new RolesDbContext(dbServiceMock.Object);
           var act=()=> context.InsertRole(new());

            //Assert
           var result = act.Should().ThrowExactly<DataException>();
        }

        [Fact]
        public void Should_Update_Role_Successfully()
        {
            // Arrange
            var roleToUpdate = new RoleModel { Id = 1, Name = "Admin" };
            var updatedRole = new RoleModel { Id = 1, Name = "Administrador" };
            var mockRoles = new List<RoleModel> { roleToUpdate };

            _dbServiceMock.Setup(x => x.ExecuteReader(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
                        .Returns(new List<Dictionary<string, dynamic>>
                        {
                            new Dictionary<string, dynamic> { { "id", 1 }, { "name", "Admin" } }
                        });

            _dbServiceMock.Setup(x => x.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()));

            // Act
            var dbContext = new RolesDbContext(_dbServiceMock.Object);
            dbContext.UpdateRoleName(1, updatedRole);

            // Assert
            _dbServiceMock.Verify(x => x.ExecuteNonQuery(
                It.Is<string>(query => query.Contains("UPDATE Roles")),
                It.Is<Dictionary<string, object>>(parameters =>
                    (int)parameters["@Id"] == 1 && (string)parameters["@Name"] == "Administrador")),
                Times.Once);
        }

                [Fact]
        public void UpdateRoleName_ThrowsDataException()
        {
            // Arrange
            var duplicateRole = new RoleModel { Id = 1, Name = "DuplicateName" };

            // Simula que hay roles existentes para que no sea null
            _dbServiceMock.Setup(x => x.ExecuteReader(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()))
                        .Returns(new List<Dictionary<string, dynamic>>
                        {
                            new() { { "id", 1 }, { "name", "Admin" } }
                        });

            var postgresException = new PostgresException("Duplicate key error", null, null, null)
            {
                Data = { ["SqlState"] = "23505" }
            };

            _dbServiceMock.Setup(x => x.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
                        .Throws(postgresException);

            var dbContext = new RolesDbContext(_dbServiceMock.Object);

            // Act
            Action act = () => dbContext.UpdateRoleName(1, duplicateRole);

            // Assert
            act.Should().Throw<DataException>()
            .WithMessage("Ya existe un rol con ese nombre.");
        }



    }
}
