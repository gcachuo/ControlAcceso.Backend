using System.Data;
using ControlAcceso.Data.Model;
using ControlAcceso.Data.Roles;
using ControlAcceso.Data.Users;
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
            //Arrange
            //Mock
            //Act
            var context = new RolesDbContext(_dbServiceMock.Object);
            context.InsertRole(new());

            //Assert
            _dbServiceMock.Verify(x=>x.ExecuteNonQuery(It.IsAny<string>(),It.IsAny<Dictionary<string,dynamic>>()));
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
    }
}