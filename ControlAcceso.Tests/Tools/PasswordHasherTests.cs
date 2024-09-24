using FluentAssertions;

namespace ControlAcceso.Tests.Tools
{
    public class PasswordHasherTests
    {
        [Theory]
        [InlineData("password", "password", true)]
        [InlineData("password", "differentPassword", false)]
        public void Should_Hash_And_Verify_Password(string password, string verifyPassword, bool expected)
        {
            //Arrange
            //Mock
            //Act
            var hashedPassword = ControlAcceso.Tools.PasswordHasher.HashPassword(password);
            var isCorrect = ControlAcceso.Tools.PasswordHasher.VerifyPassword(verifyPassword, hashedPassword);

            //Assert
            isCorrect.Should().Be(expected);
        }
    }
}