using ControlAcceso.Data.Packages;
using ControlAcceso.Services.DBService;
using Moq;
using Npgsql;



namespace ControlAcceso.Tests
{
    public class PackagesDbContextTests
    {
        private readonly Mock<IDbService> _mockDbService;
        private readonly IPackagesDbContext _dbContext;

        public PackagesDbContextTests()
        {
            _mockDbService = new Mock<IDbService>();
            _dbContext = new PackagesDbContext(_mockDbService.Object);
        }

        [Fact]
        public void GetReceivedPackages_ReturnsListOfPackages()
        {
            // Arrange
            var mockData = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "id", 1 },
                    { "service", "Delivery" },
                    { "received_at", DateTime.Now },
                    { "confirmed_at", DBNull.Value },
                    { "address_id", 101 },
                    { "status", 0 }
                },
                new Dictionary<string, object>
                {
                    { "id", 2 },
                    { "service", "Pickup" },
                    { "received_at", DateTime.Now },
                    { "confirmed_at", DBNull.Value },
                    { "address_id", 102 },
                    { "status", 0 }
                }
            };

            _mockDbService.Setup(x => x.ExecuteReader(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()))
                          .Returns(mockData);

            // Act
            var result = _dbContext.SelectPackages();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].Id);
            Assert.Equal("Delivery", result[0].Service);
            Assert.Equal(101, result[0].AddressId);
        }

        [Fact]
        public void GetReceivedPackages_ReturnsEmptyList_WhenNoPackages()
        {
            // Arrange
            _mockDbService.Setup(x => x.ExecuteReader(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()))
                          .Returns(new List<Dictionary<string, object>>());

            // Act
            var result = _dbContext.SelectPackages();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetReceivedPackages_ThrowsException_WhenDbFails()
        {
            // Arrange
            _mockDbService.Setup(x => x.ExecuteReader(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()))
                          .Throws(new Exception("Database connection failed"));

            // Act & Assert
            Assert.Throws<Exception>(() => _dbContext.SelectPackages());
        }
    }
}
