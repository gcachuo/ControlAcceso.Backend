using System.Collections.Generic;
using ControlAcceso.Data.Permissions;
using ControlAcceso.Data.Roles;
using ControlAcceso.Data.Model;
using ControlAcceso.Endpoints.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Moq;

namespace ControlAcceso.Tests.Endpoints
{
    public class PermissionsEndpointTests
    {
        private readonly Mock<IPermissionsDbContext> _mockPermissionsDbContext = new(MockBehavior.Strict);
        private readonly Mock<IRolesDbContext> _mockRolesDbContext = new(MockBehavior.Strict);

        [Fact]
        public void Should_Get_Permissions_Successfully()
        {
            // Arrange
            var expectedPermissions = new Dictionary<string, List<string>>
            {
                {"Entity1",new List<string>(){ "Read", "Write" } },
                { "Entity2", new List<string> { "Execute" } }
            };

            _mockPermissionsDbContext.Setup(x => x.GetGroupedPermissions(It.IsAny<int>(), It.IsAny<int>()))
                                      .Returns(expectedPermissions);

            var endpoint = new ControlAcceso.Endpoints.Roles.Endpoint(_mockRolesDbContext.Object, _mockPermissionsDbContext.Object);

            // Act
            var result = endpoint.GetPermissions(1, 1) as OkObjectResult;

            // Assert
            result?.StatusCode.Should().Be(StatusCodes.Status200OK, result.Value?.ToString());
            var response = result?.Value as PermissionsResponse;

            response!.Message.Should().Be("OK");
            response!.Permissions.Should().NotBeNull();
            response!.Permissions.Should().BeEquivalentTo(expectedPermissions);
        }

        [Fact]
        public void Should_Return_NotFound_When_No_Permissions()
        {
            // Arrange
            _mockPermissionsDbContext.Setup(x => x.GetGroupedPermissions(It.IsAny<int>(), It.IsAny<int>()))
                                      .Returns(new Dictionary<string, List<string>>());

            var endpoint = new ControlAcceso.Endpoints.Roles.Endpoint(_mockRolesDbContext.Object, _mockPermissionsDbContext.Object);

            // Act
            var result = endpoint.GetPermissions(1, 1) as NotFoundObjectResult;

            // Assert
            result?.StatusCode.Should().Be(StatusCodes.Status404NotFound, result.Value?.ToString());
            var response = result!.Value as PermissionsResponse;

            response!.Message.Should().Be("No se encontraron permisos.");
            response.Permissions.Should().NotBeNull();
            response.Permissions.Should().BeEmpty();
        }
    }
}
