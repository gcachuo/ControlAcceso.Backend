using Moq;
using Xunit;
using ControlAcceso.Data.Permissions;
using ControlAcceso.Services.DBService;
using Npgsql;

namespace ControlAcceso.Tests
{
    public class PermissionsDbContextTests
    {
        private readonly Mock<IDbService> _mockDbService;
        private readonly IPermissionsDbContext _dbContext;

        public PermissionsDbContextTests()
        {
            _mockDbService = new Mock<IDbService>();
            _dbContext = new PermissionsDbContext(_mockDbService.Object);
        }

        [Fact]
        public void GetGroupedPermissions_ReturnsListOfGroupedPermissions()
        {
            // Arrange
            var mockData = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "entity", "Entity1" },
                    { "permissions", new[] { "Read", "Write" } }
                },
                new Dictionary<string, object>
                {
                    { "entity", "Entity2" },
                    { "permissions", new[] { "Execute" } }
                }
            };

            _mockDbService.Setup(x => x.ExecuteReader(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()))
                          .Returns(mockData);

            // Act
            var result = _dbContext.GetGroupedPermissions(1, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Entity1", result[0].Entity);
            Assert.Contains("Read", result[0].Permissions);
            Assert.Contains("Write", result[0].Permissions);
        }

        [Fact]
        public void GetGroupedPermissions_ReturnsEmptyList_WhenNoPermissions()
        {
            // Arrange
            _mockDbService.Setup(x => x.ExecuteReader(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()))
                          .Returns(new List<Dictionary<string, object>>());

            // Act
            var result = _dbContext.GetGroupedPermissions(1, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetGroupedPermissions_HandlesException_Gracefully()
        {
            // Arrange
            var mockDbService = new Mock<IDbService>();

            mockDbService.Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>() ))
                        .Throws(new PostgresException("Database connection failed", "Severity", "InvariantSeverity", "SqlState"));

            var dbContext = new PermissionsDbContext(mockDbService.Object);

            // Act & Assert
            var exception = Assert.Throws<PostgresException>(() => dbContext.GetGroupedPermissions(1, 1));

            Assert.Equal("SqlState: Database connection failed", exception.Message);
        }
    }
}
