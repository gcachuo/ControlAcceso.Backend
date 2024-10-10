using System.Data;
using ControlAcceso.Data.Model;
using ControlAcceso.Data.RefreshTokens;
using ControlAcceso.Data.Users;
using ControlAcceso.Endpoints.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Moq;
using Npgsql;
using Endpoint = ControlAcceso.Endpoints.Users.Endpoint;

namespace ControlAcceso.Tests.Endpoints
{
    public class UsersTests
    {
        private readonly Mock<IUsersDbContext> _usersDbContext = new(MockBehavior.Strict);
        private readonly Mock<IRefreshTokensDbContext> _refreshTokensDbContext = new(MockBehavior.Strict);

        [Fact]
        public void Should_Register_User_Successfully()
        {
            //Arrange
            var request = new UserRequest() { Password = "123456" };

            //Mock
            _usersDbContext.Setup(x => x.InsertUser(It.IsAny<UserModel>()));

            //Act
            var endpoint = new Endpoint(_usersDbContext.Object, _refreshTokensDbContext.Object);
            var result = endpoint.RegisterUser(request) as ObjectResult;
            ;

            //Assert
            result?.StatusCode.Should().Be(StatusCodes.Status200OK, result.Value?.ToString());
            (result!.Value as UserResponse)!.Message.Should().Be("OK");
        }

        [Fact]
        public void When_User_Is_Already_Registered_Then_Exception_Is_Thrown()
        {
            //Arrange
            var request = new UserRequest() { Password = "123456" };

            //Mock
            _usersDbContext.Setup(x => x.InsertUser(It.IsAny<UserModel>())).Throws<DataException>(() => new("Usuario duplicado."));

            //Act
            var endpoint = new Endpoint(_usersDbContext.Object, _refreshTokensDbContext.Object);
            var result = endpoint.RegisterUser(request) as ObjectResult;
            ;

            //Assert
            result?.StatusCode.Should().Be(StatusCodes.Status400BadRequest, result.Value?.ToString());
            (result!.Value as UserResponse)!.Message.Should().Be("Usuario duplicado.");
        }

        [Fact]
        public void Should_Edit_User_Successfully()
        {
            //Arrange
            const int idUser = 1;

            //Mock
            var request = new UserRequest() { Password = "123456" };
            // Mock setup for UpdateUser

            _usersDbContext.Setup(x => x.UpdateUser(It.IsAny<UserModel>(), It.IsAny<int>()))
                .Verifiable(); // Verifica que el método fue llamado
            // Suponiendo que el método UpdateUser retorna un booleano


            //Act
            var endpoint = new Endpoint(_usersDbContext.Object, _refreshTokensDbContext.Object);
            var result = endpoint.EditUser(idUser, request) as ObjectResult;

            //Assert
            result?.StatusCode.Should().Be(StatusCodes.Status200OK, result.Value?.ToString());
            (result!.Value as UserResponse)!.Message.Should().Be("Usuario actualizado correctamente");

            //Verifica que el mock haya sido llamado
            _usersDbContext.Verify(x => x.UpdateUser(It.IsAny<UserModel>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Should_Get_User_Successfully()
        {
            //Arrange
            const int idUser = 1;

            //Mock
            _usersDbContext.Setup(x => x.SelectUser(idUser)).Returns(new UserModel());

            //Act
            var endpoint = new Endpoint(_usersDbContext.Object, _refreshTokensDbContext.Object);
            var result = endpoint.GetUser(idUser) as ObjectResult;

            //Assert
            result?.StatusCode.Should().Be(StatusCodes.Status200OK, result.Value?.ToString());
            (result!.Value as UserResponse)!.Message.Should().Be("OK");
        }
    }
}