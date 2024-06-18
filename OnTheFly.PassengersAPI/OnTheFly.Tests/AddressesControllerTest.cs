using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Moq;
using OnTheFly.AddressAPI.Controllers;
using OnTheFly.AddressAPI.Data;
using OnTheFly.AddressAPI.PostalServices;

namespace OnTheFly.Tests
{
    public class AddressesControllerTest
    {
        private readonly DbContextOptions<OnTheFlyAddressAPIContext> _options;

        public AddressesControllerTest()
        {
            _options = new DbContextOptionsBuilder<OnTheFlyAddressAPIContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new OnTheFlyAddressAPIContext(_options);

            context.Address.Add(new Address
            {
                City = "Matão",
                Complement = "Casa B",
                Number = "2",
                State = "SP",
                Street = "Rua Sinharia Frota",
                ZipCode = "15990-220"
            });

            context.SaveChanges();
        }

        [Fact]
        public async Task Get_ReturnsAllAddresses()
        {
            var context = new OnTheFlyAddressAPIContext(_options);
            var service = new ViaCepService();

            var controller = new AddressesController(context, service);

            var result = await controller.Get();

            if (result.Result is OkObjectResult okObject)
            {
                var addresses = Assert.IsAssignableFrom<IEnumerable<Address>>(okObject.Value);
                Assert.Equal(200, okObject.StatusCode);
                Assert.IsType<List<Address>>(addresses);
                Assert.Equal(1, addresses.Count());
            }
        }

        [Fact]
        public async Task Get_ValidZipcode_ReturnsAddress()
        {
            var context = new OnTheFlyAddressAPIContext(_options);
            var service = new ViaCepService();

            var controller = new AddressesController(context, service);

            var result = await controller.Get("15990220", "2");

            if (result.Result is OkObjectResult okObject)
            {
                var address = Assert.IsAssignableFrom<Address>(okObject.Value);
                Assert.Equal(200, okObject.StatusCode);
                Assert.IsType<Address>(address);
                Assert.Equal("15990-220", address.ZipCode);
            }
        }

        [Fact]
        public async Task Get_InvalidZipcode_ReturnsNotFound()
        {
            var context = new OnTheFlyAddressAPIContext(_options);
            var service = new ViaCepService();

            var controller = new AddressesController(context, service);

            var result = await controller.Get("159902220", "2");

            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task Post_ValidAddress_ReturnsCreatedResponse()
        {
            var context = new OnTheFlyAddressAPIContext(_options);
            var service = new ViaCepService();

            var controller = new AddressesController(context, service);

            var addressDto = new AddressDTO
            {
                Complement = "Apto 101",
                Number = "2",
                ZipCode = "15990-840"
            };

            var createdResult = await controller.Post(addressDto);

            var createdAddress = Assert.IsType<Address>(createdResult.Value);
            Assert.Equal(addressDto.Complement, createdAddress.Complement);
            Assert.Equal(addressDto.Number, createdAddress.Number);
        }

        [Fact]
        public async Task Post_ValidAddressExistsInDatabase_ReturnsConflict()
        {
            var context = new OnTheFlyAddressAPIContext(_options);
            var service = new ViaCepService();

            var controller = new AddressesController(context, service);

            var addressDto = new AddressDTO
            {
                Complement = "Apto 101",
                Number = "2",
                ZipCode = "15990-220"
            };

            var createdResult = await controller.Post(addressDto);

            var conflictResult = Assert.IsType<ConflictObjectResult>(createdResult.Result);
            Assert.Equal(409, conflictResult.StatusCode);
        }

        [Fact]
        public async Task Post_InvalidAddress_ReturnsBadRequest()
        {
            var context = new OnTheFlyAddressAPIContext(_options);
            var service = new ViaCepService();

            var controller = new AddressesController(context, service);

            var addressDto = new AddressDTO
            {
                Complement = "Apto 101",
                Number = "2",
                ZipCode = "15990-2220"
            };

            var createdResult = await controller.Post(addressDto);

            var badRequestResult = Assert.IsType<BadRequestResult>(createdResult.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
    }
}
