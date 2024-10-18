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
            var expectedAddresses = new List<AddressModel>
            {
                new AddressModel { Street = "Main St", Number = "123" },
                new AddressModel { Street = "Second St", Number = "456" }
            };

            _addressesDbContext.Setup(x => x.SelectAddress()).Returns(expectedAddresses);

            var endpoint = new ControlAcceso.Endpoints.Addresses.Endpoint(_addressesDbContext.Object); 

            // Act
            var result = endpoint.GetAddress() as ObjectResult;

            // Assert
            result?.StatusCode.Should().Be(StatusCodes.Status200OK, result.Value?.ToString());
            
            var response = result?.Value as Response;
            
            response!.Message.Should().Be("OK");
            
            response!.Addresses.Should().NotBeNull();
            response!.Addresses.Should().BeEquivalentTo(expectedAddresses);
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
            
            var response = result!.Value as Response; 
            response!.Message.Should().Be("OK");
            
            response.Addresses.Should().NotBeNull();
            response.Addresses.Should().BeEmpty();
        }

    }
}
