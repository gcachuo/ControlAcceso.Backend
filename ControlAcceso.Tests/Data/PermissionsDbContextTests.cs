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
        public void GetGroupedPermissions_ShouldReturnPermissions_WhenDataExists()
        {
            // Arrange
            var roleId = 1;
            var userId = 2;
            
            var mockData = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "entity", "Users" },
                    { "permissions", new string[] { "Read", "Write" } }
                },
                new Dictionary<string, object>
                {
                    { "entity", "Orders" },
                    { "permissions", new string[] { "Execute" } }
                }
            };

            _mockDbService.Setup(db => db.ExecuteReader(
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, dynamic>>()
            )).Returns(mockData);

            // Act
            var result = _dbContext.GetGroupedPermissions(roleId, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.True(result.ContainsKey("Users"));
            Assert.Equal(new List<string> { "Read", "Write" }, result["Users"]);

            Assert.True(result.ContainsKey("Orders"));
            Assert.Equal(new List<string> { "Execute" }, result["Orders"]);
        }

        [Fact]
        public void GetGroupedPermissions_ShouldReturnEmptyDictionary_WhenNoDataExists()
        {
            // Arrange
            var roleId = 3;
            var userId = 4;

            _mockDbService.Setup(db => db.ExecuteReader(
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, dynamic>>()
            )).Returns(new List<Dictionary<string, object>>());

            // Act
            var result = _dbContext.GetGroupedPermissions(roleId, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
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

        [Fact]
        public void GetRolePermissions_ShouldReturnGroupedPermissions_WhenDataExists()
        {
            // Arrange
            var roleId = 1;
            var mockData = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "entity", "Users" },
                    { "permissions", new string[] { "Read", "Write" } }
                },
                new Dictionary<string, object>
                {
                    { "entity", "Orders" },
                    { "permissions", new string[] { "Execute" } }
                }
            };

            _mockDbService.Setup(db => db.ExecuteReader(
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, dynamic>>()
            )).Returns(mockData);

            // Act
            var result = _dbContext.GetRolePermissions(roleId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.True(result.ContainsKey("Users"));
            Assert.Equal(new List<string> { "Read", "Write" }, result["Users"]);
            Assert.True(result.ContainsKey("Orders"));
            Assert.Equal(new List<string> { "Execute" }, result["Orders"]);
        }

        [Fact]
        public void GetRolePermissions_ShouldReturnEmptyDictionary_WhenNoDataExists()
        {
            // Arrange
            var roleId = 2;
            _mockDbService.Setup(db => db.ExecuteReader(
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, dynamic>>()
            )).Returns(new List<Dictionary<string, object>>());

            // Act
            var result = _dbContext.GetRolePermissions(roleId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
