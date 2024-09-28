using ControlAcceso.Data.Model;
using ControlAcceso.Data.Roles;
using ControlAcceso.Endpoints.Roles;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Endpoint = ControlAcceso.Endpoints.Roles.Endpoint;

namespace ControlAcceso.Tests.Endpoints
{
    public class RolesTests
    {
        private readonly Mock<IRolesDbContext> _dbContextMock = new(MockBehavior.Strict);

        [Fact]
        public void Should_Create_Role_Successfully()
        {
            //Arrange
            var request = new Request { Name = "Test" };

            //Mock
            _dbContextMock
                .Setup(x => x.InsertRole(It.IsAny<RoleModel>()));

            //Act
            var endpoint = new Endpoint(_dbContextMock.Object);
            var result = endpoint.CreateRole(request) as ObjectResult;

            //Assert
            result?.StatusCode.Should().Be(StatusCodes.Status200OK, result.Value?.ToString());
            (result!.Value as Response)!.Message.Should().Be("OK");
        }
    }
}