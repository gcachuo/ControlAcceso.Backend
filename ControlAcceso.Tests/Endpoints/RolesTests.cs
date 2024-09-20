using ControlAcceso.Endpoints.Roles;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Endpoint = ControlAcceso.Endpoints.Roles.Endpoint;

namespace ControlAcceso.Tests.Endpoints
{
    public class RolesTests
    {
        [Fact]
        public void Should_Create_Role()
        {
            //Arrange
            //Mock
            //Act
            var endpoint = new Endpoint();
            var result = endpoint.CreateRole() as ObjectResult;;

            //Assert
            result?.StatusCode.Should().Be(StatusCodes.Status200OK, result.Value?.ToString());
            (result!.Value as Response)!.Message.Should().Be("OK");
        }
    }
}