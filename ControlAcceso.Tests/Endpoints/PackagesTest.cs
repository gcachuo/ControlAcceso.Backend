using System.Data;
using System.Collections.Generic;
using ControlAcceso.Data.Model;
using ControlAcceso.Data.Packages;
using ControlAcceso.Endpoints.Packages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Moq;

namespace ControlAcceso.Tests.Endpoints
{
    public class PackageTests
    {
        private readonly Mock<IPackagesDbContext> _mockPackageDbContext = new(MockBehavior.Strict);

        [Fact]
        public void Should_Get_Received_Packages_Successfully()
        {
            // Arrange
            var expectedPackages = new List<PackageModel>
            {
                new PackageModel { Id = 1, Service = "Delivery", ReceivedAt = DateTime.Now, ConfirmedAt = null, Status = 0, AddressId = 1 },
                new PackageModel { Id = 2, Service = "Pickup", ReceivedAt = DateTime.Now, ConfirmedAt = null, Status = 0, AddressId = 2 }
            };

            _mockPackageDbContext.Setup(x => x.SelectPackages()).Returns(expectedPackages);

            // Act
            var endpoint = new ControlAcceso.Endpoints.Packages.Endpoint(_mockPackageDbContext.Object);
            var result = endpoint.GetPackageList() as OkObjectResult;

            // Assert
            result?.StatusCode.Should().Be(StatusCodes.Status200OK, result.Value?.ToString());
            var response = result?.Value as Response;

            response!.Message.Should().Be("OK");
            response!.Packages.Should().NotBeNull();
            response!.Packages.Should().BeEquivalentTo(expectedPackages);
        }

        [Fact]
        public void Should_Return_Empty_List_When_No_Packages()
        {
            // Arrange
            _mockPackageDbContext.Setup(x => x.SelectPackages()).Returns(new List<PackageModel>());

            // Act
            var endpoint = new ControlAcceso.Endpoints.Packages.Endpoint(_mockPackageDbContext.Object);
            var result = endpoint.GetPackageList() as OkObjectResult;

            // Assert
            result?.StatusCode.Should().Be(StatusCodes.Status200OK, result.Value?.ToString());
            var response = result!.Value as Response;

            response!.Message.Should().Be("OK");
            response.Packages.Should().NotBeNull();
            response.Packages.Should().BeEmpty();
        }
    }
}
