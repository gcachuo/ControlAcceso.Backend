using ControlAcceso.Data.Model;
using ControlAcceso.Data.Addresses;
using ControlAcceso.Endpoints.Addresses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Moq;

namespace ControlAcceso.Tests.Endpoints
{
    public class AddressesTests 
    {
        private readonly Mock<IAddressesDbContext> _addressesDbContext = new(MockBehavior.Strict);

        [Fact]
        public void Should_Get_Address_Successfully()
        {
            // Arrange
            _addressesDbContext.Setup(x => x.SelectAddress()).Returns(new List<AddressModel>());

            // Act
            var endpoint = new ControlAcceso.Endpoints.Addresses.Endpoint(_addressesDbContext.Object); 
            var result = endpoint.GetAddress() as ObjectResult;

            // Assert
            result?.StatusCode.Should().Be(StatusCodes.Status200OK, result.Value?.ToString());
            (result!.Value as Response)!.Message.Should().Be("OK");
        }

        [Fact]
        public void Should_Not_Get_Address()
        {
            // Arrange
            _addressesDbContext.Setup(x => x.SelectAddress()).Returns(new List<AddressModel>()); 

            // Act
            var endpoint = new ControlAcceso.Endpoints.Addresses.Endpoint(_addressesDbContext.Object);
            var result = endpoint.GetAddress() as ObjectResult;

            // Assert
            result?.StatusCode.Should().Be(StatusCodes.Status200OK, result.Value?.ToString());
            (result!.Value as Response)!.Message.Should().Be("OK"); 
        }

    }
}
