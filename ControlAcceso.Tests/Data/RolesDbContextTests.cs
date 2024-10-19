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
    }
}
