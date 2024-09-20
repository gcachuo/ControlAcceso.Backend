using ControlAcceso.Endpoints.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Endpoint = ControlAcceso.Endpoints.Users.Endpoint;

namespace ControlAcceso.Tests.Endpoints
{
    public class UsersTests
    {
        [Fact]
        public void Should_Register_User_Successfully()
        {
            //Arrange
            //Mock
            //Act
            var endpoint = new Endpoint();
            var result = endpoint.RegisterUser() as ObjectResult;;

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
            //Act
            var endpoint = new Endpoint();
            var result = endpoint.EditUser(idUser) as ObjectResult;;

            //Assert
            result?.StatusCode.Should().Be(StatusCodes.Status200OK, result.Value?.ToString());
            (result!.Value as Response)!.Message.Should().Be("OK");
        }
    }
}