using ControlAcceso.Data.Addresses;
using ControlAcceso.Services.DBService;
using Moq;
using Npgsql;

public class AddressesDbContextTests
{
    [Fact]
public void SelectAddress_ReturnsListOfAddresses_WhenDataIsValid()
{
    // Arrange
    var mockDbService = new Mock<IDbService>();

    var fakeRows = new List<Dictionary<string, dynamic>>()
    {
        new Dictionary<string, dynamic> { { "street", "Main St" }, { "number", "123" } },
        new Dictionary<string, dynamic> { { "street", "Second St" }, { "number", "456" } }
    };

    mockDbService.Setup(db => db.ExecuteReader("SELECT * FROM addresses", It.IsAny<Dictionary<string, dynamic>>()))
                 .Returns(fakeRows);

    var dbContext = new AddressesDbContext(mockDbService.Object);

    // Act
    var result = dbContext.SelectAddress();

    // Assert
    Assert.NotNull(result); 
    Assert.Equal(2, result.Count());

    var expectedAddresses = new List<(string Street, string Number)>
    {
        ("Main St", "123"),
        ("Second St", "456")
    };

    foreach (var (expected, actual) in expectedAddresses.Zip(result, (e, a) => (e, a)))
    {
        Assert.Equal(expected.Street, actual.Street);
        Assert.Equal(expected.Number, actual.Number);
    }
}

    [Fact]
    public void SelectAddress_ReturnsEmptyList()
    {
        // Arrange
        var mockDbService = new Mock<IDbService>();

        mockDbService.Setup(db => db.ExecuteReader("SELECT * FROM addresses", It.IsAny<Dictionary<string, dynamic>>()))
                     .Returns(new List<Dictionary<string, dynamic>>());

        var dbContext = new AddressesDbContext(mockDbService.Object);

        // Act
        var result = dbContext.SelectAddress();

        // Assert
        Assert.NotNull(result); 
        Assert.Empty(result);
    }

    [Fact]
    public void SelectAddress_HandlesException_Gracefully()
    {
        // Arrange
        var mockDbService = new Mock<IDbService>();

        
        mockDbService.Setup(db => db.ExecuteReader("SELECT * FROM addresses", It.IsAny<Dictionary<string, dynamic>>()))
                     .Throws(new PostgresException("Database error", "Severity", "invariantSeverity","sqlState"));

        var dbContext = new AddressesDbContext(mockDbService.Object);

         // Act & Assert
        var exception = Assert.Throws<PostgresException>(() => dbContext.SelectAddress());

        // Ajustar la comparación al mensaje completo
        Assert.Equal("sqlState: Database error", exception.Message);
            
    }
}
