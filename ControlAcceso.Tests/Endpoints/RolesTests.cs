using ControlAcceso.Data.Model;
using ControlAcceso.Data.Roles;
using ControlAcceso.Data.Permissions;
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
        private readonly Mock<IPermissionsDbContext> _mockPermissionsDbContext = new(MockBehavior.Strict);
        private readonly Mock<IRolesDbContext> _mockRolesdbContext = new(MockBehavior.Strict);

        [Fact]
        public void Should_Create_Role_Successfully()
        {
            //Arrange
            var request = new Request { Name = "Test" };

            //Mock
            _mockRolesdbContext
                .Setup(x => x.InsertRole(It.IsAny<RoleModel>()));

            //Act
            var endpoint = new Endpoint(_mockRolesdbContext.Object, _mockPermissionsDbContext.Object);
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

            _mockRolesdbContext.Setup(x => x.SelectRole()).Returns(mockRoles);

            // Act
            var endpoint = new Endpoint(_mockRolesdbContext.Object, _mockPermissionsDbContext.Object);
            var result = endpoint.GetRoleList() as ObjectResult;

            // Assert
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            (result!.Value as RoleResponse)!.Roles.Should().BeEquivalentTo(mockRoles); 
        }


        [Fact]
        public void Should_Handle_Empty_Role_List()
        {
            // Arrange
            var mockRoles = new List<RoleModel>(); 

            _mockRolesdbContext.Setup(x => x.SelectRole()).Returns(mockRoles);

            // Act
            var endpoint = new Endpoint(_mockRolesdbContext.Object, _mockPermissionsDbContext.Object);
            var result = endpoint.GetRoleList() as ObjectResult;

            // Assert
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            (result!.Value as RoleResponse)!.Roles.Should().BeEmpty(); 
        }

        [Fact]
        public void Should_Edit_Role_Successfully()
        {
            // Arrange
            var request = new Request { Name = "NuevoNombre" };
            var mockRoles = new List<RoleModel>
            {
                new RoleModel { Id = 1, Name = "Admin" }
            };

            _rolesDbContext.Setup(x => x.SelectRole()).Returns(mockRoles);
            _rolesDbContext.Setup(x => x.UpdateRoleName(It.IsAny<int>(), It.IsAny<RoleModel>()));

            // Act
            var endpoint = new Endpoint(_rolesDbContext.Object,_mockPermissionsDbContext.Object);
            var result = endpoint.EditRole(1, request) as ObjectResult;

            // Assert
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
            (result!.Value as Response)!.Message.Should().Be("Rol actualizado exitosamente.");
        }

        [Fact]
        public void Should_Return_BadRequest_If_Role_Is_Null()
        {
            // Arrange
            var request = new Request { Name = "" }; 
            var mockRoles = new List<RoleModel>(); 

            // Setup the mock
            _rolesDbContext.Setup(x => x.SelectRole()).Returns(mockRoles); 

            // Act
            var endpoint = new Endpoint(_rolesDbContext.Object,_mockPermissionsDbContext.Object);
            var result = endpoint.EditRole(1, request) as ObjectResult;

            // Assert
            result?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            (result!.Value as Response)!.Message.Should().Be("El campo 'name' es obligatorio"); 
        }

    }
}
