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
   
        private readonly Mock<IRolesDbContext> _rolesDbContext = new(MockBehavior.Strict);

        [Fact]
        public void Should_Get_Roles_Successfully()
        {
            // Arrange
            var mockRoles = new List<RoleModel>
            {
                new RoleModel { Name = "Admin" },
                new RoleModel { Name = "User" }
            };

            _rolesDbContext.Setup(x => x.SelectRole()).Returns(mockRoles);

            // Act
            var endpoint = new ControlAcceso.Endpoints.Roles.Endpoint(_rolesDbContext.Object);
            var result = endpoint.GetRoleList() as ObjectResult;

            // Assert
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            (result!.Value as Response)!.Roles.Should().BeEquivalentTo(mockRoles); 
        }


        [Fact]
        public void Should_Handle_Empty_Role_List()
        {
            // Arrange
            var mockRoles = new List<RoleModel>(); 

            _rolesDbContext.Setup(x => x.SelectRole()).Returns(mockRoles);

            // Act
            var endpoint = new ControlAcceso.Endpoints.Roles.Endpoint(_rolesDbContext.Object);
            var result = endpoint.GetRoleList() as ObjectResult;

            // Assert
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            (result!.Value as Response)!.Roles.Should().BeEmpty(); 
        }

    }
}
