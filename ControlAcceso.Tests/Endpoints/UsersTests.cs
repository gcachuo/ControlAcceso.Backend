using ControlAcceso.Data.Model;
using ControlAcceso.Data.Users;
using ControlAcceso.Endpoints.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Moq;
using Endpoint = ControlAcceso.Endpoints.Users.Endpoint;

namespace ControlAcceso.Tests.Endpoints
{
    public class UsersTests
    {
        private readonly Mock<IUsersDbContext> _usersDbContext=new(MockBehavior.Strict);
        
        [Fact]
        public void Should_Register_User_Successfully()
        {
            //Arrange
            var request = new Request(){Password = "123456"};
            
            //Mock
            _usersDbContext.Setup(x => x.InsertUser(It.IsAny<UserModel>()));
            
            //Act
            var endpoint = new Endpoint(_usersDbContext.Object);
            var result = endpoint.RegisterUser(request) as ObjectResult;;

            //Assert
            result?.StatusCode.Should().Be(StatusCodes.Status200OK, result.Value?.ToString());
            (result!.Value as Response)!.Message.Should().Be("OK");
        }
        
        [Fact]
        public void Should_Edit_User_Successfully()
        {
            //Arrange
            const string idUser = "1";

            //Mock
            var request = new Request(){Password = "123456"};
            
            //Act
            var endpoint = new Endpoint(_usersDbContext.Object);
            var result = endpoint.EditUser(idUser, request) as ObjectResult;

            //Assert
            result?.StatusCode.Should().Be(StatusCodes.Status200OK, result.Value?.ToString());
            (result!.Value as Response)!.Message.Should().Be("OK");
        }
    }
}