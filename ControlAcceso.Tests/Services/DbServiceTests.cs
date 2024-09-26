using System.Data;
using ControlAcceso.Services.DBService;
using Microsoft.Extensions.Logging;
using Moq;

namespace ControlAcceso.Tests.Services
{
    public class DbServiceTests
    {
        private readonly Mock<ILogger<DbService>> _loggerMock = new(MockBehavior.Default);
        private readonly Mock<IDbConnection> _dbConnectionMock=new(MockBehavior.Default);
        private readonly Mock<IDbCommand> _commandMock = new(MockBehavior.Default);
        private readonly Mock<IDataParameterCollection> _parameterMock = new(MockBehavior.Default);

        [Fact]
        public void Should_Insert_Row_Successfully()
        {
            //Arrange
            //Mock
            _dbConnectionMock.Setup(conn => conn.CreateCommand()).Returns(_commandMock.Object);
            _commandMock.Setup(cmd => cmd.Parameters).Returns(_parameterMock.Object);
            _commandMock.Setup(cmd => cmd.ExecuteNonQuery()).Returns(1);
            
            //Act
            var service = new DbService(_loggerMock.Object, _dbConnectionMock.Object);
            service.ExecuteNonQuery("INSERT INTO users DEFAULT VALUES",new());

            //Assert
            _dbConnectionMock.Verify(conn => conn.Open(), Times.Once);
            _dbConnectionMock.Verify(conn => conn.Close(), Times.Once);
            _commandMock.Verify(cmd => cmd.ExecuteNonQuery(), Times.Once);
        }
    }
}