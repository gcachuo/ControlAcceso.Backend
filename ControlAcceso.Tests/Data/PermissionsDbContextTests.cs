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
        public void GetGroupedPermissions_ReturnsDictionaryOfPermissions()
        {
            // Arrange
            var mockData = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "entity", "users" }, 
                    { "permissions", new List<string> { "Read", "Write" } }
                },
                new Dictionary<string, object>
                {
                    { "entity", "package" }, 
                    { "permissions", new List<string> { "Delete" } }
                }
            };

            _mockDbService.Setup(x => x.ExecuteReader(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>() ))
                        .Returns(mockData);

            // Act
            var result = _dbContext.GetGroupedPermissions(1, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.True(result.ContainsKey("users"));
            Assert.Contains("Read", (List<string>)result["users"]);
            Assert.Contains("Write", (List<string>)result["users"]);

            Assert.True(result.ContainsKey("package"));
            Assert.Contains("Delete", (List<string>)result["package"]);
        }



        [Fact]
        public void GetGroupedPermissions_ReturnsEmptyDictionary_WhenNoPermissions()
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
            _mockDbService.Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()))
                          .Throws(new PostgresException("Database connection failed", "Severity", "InvariantSeverity", "SqlState"));

            // Act & Assert
            var exception = Assert.Throws<PostgresException>(() => _dbContext.GetGroupedPermissions(1, 1));

            Assert.Equal("SqlState: Database connection failed", exception.Message);
        }
    }
}
