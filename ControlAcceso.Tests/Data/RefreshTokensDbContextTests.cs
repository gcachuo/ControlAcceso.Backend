using ControlAcceso.Data.RefreshTokens;
using ControlAcceso.Services.DBService;
using Moq;

namespace ControlAcceso.Tests.Data
{
    public class RefreshTokensDbContextTests
    {
        private readonly Mock<IDbService> _dbServiceMock = new(MockBehavior.Default);

        [Fact]
        public void Should_Insert_Role_Successfully()
        {
            //Arrange
            const string refreshToken = "";
            const int  userId = 1;
            const string  ipAddress = "";
            const string  userAgent = "";
            
            //Mock
            //Act
            var context = new RefreshTokensDbContext(_dbServiceMock.Object);
            context.InsertToken(refreshToken,userId,ipAddress,userAgent);

            //Assert
            _dbServiceMock.Verify(x=>x.ExecuteNonQuery(It.IsAny<string>(),It.IsAny<Dictionary<string,dynamic>>()));
        }
    }
}