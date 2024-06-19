using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Moq;
using OnTheFly.PassengersAPI.Controllers;
using OnTheFly.PassengersAPI.Data;
using Services;

namespace OnTheFly.Tests
{
    public class CreatePassengerTest
    {
        private readonly DbContextOptions<OnTheFlyPassengersAPIContext> _options;
        private readonly CreatePassengerService _createPassengerService;
        private readonly GetPassengerService _getPassengerService;
        private readonly UpdatePassengerService _updatePassengerService;
        private readonly DeletePassengerService _deletePassengerService;

        CreatePassengerTest()
        {
            _options = new DbContextOptionsBuilder<OnTheFlyPassengersAPIContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _createPassengerService = new CreatePassengerService();
            _getPassengerService = new GetPassengerService();
            _updatePassengerService = new UpdatePassengerService();
            _deletePassengerService = new DeletePassengerService();

            var context = new OnTheFlyPassengersAPIContext(_options);

            context.Passenger.Add(new Passenger
            {
                Cpf = "745.933.040-08",
                Name = "Gustavo Vono",
                DtBirth = new DateTime(1997, 1, 3),
                DtRegister = DateTime.Now,
                Address = new Address
                {
                    Street = "Rua Alberto Augusto Teixeira",
                    Number = "123",
                    City = "Jaú",
                    State = "SP",
                    Complement = "My House"
                },
                AddressNumber = "123",
                AddressZipCode = "17212-310"
            });

            context.SaveChanges();
        }

        [Fact]
        public async Task PostPassenger_ReturnsOk()
        {
            var context = new OnTheFlyPassengersAPIContext(_options);
            var controller = new PassengersController(context, _updatePassengerService, _createPassengerService, _getPassengerService, _deletePassengerService);

            var passengerDTO = new PassengerDTO
            {
                Cpf = "887.265.430-01",
                Name = "Gustavo Vono",
                DtBirth = new DateTime(1997, 1, 3),
                AddressDTO = new AddressDTO
                {
                    ZipCode = "17212-310",
                    Number = "123",
                    Complement = "My House"
                }
            };

            var result = await controller.PostPassenger(passengerDTO);

            var createdPassenger = Assert.IsType<Passenger>(result.Value);
            Assert.Equal(passengerDTO.Name, createdPassenger.Name);
            Assert.Equal(passengerDTO.Cpf, createdPassenger.Cpf);
        }

        [Fact]
        public async Task PostPassenger_ReturnsBadRequest()
        {
            var context = new OnTheFlyPassengersAPIContext(_options);
            var controller = new PassengersController(context, _updatePassengerService, _createPassengerService, _getPassengerService, _deletePassengerService);

            var passengerDTO = new PassengerDTO
            {
                Cpf = "745.933.040-08",
                Name = "Gustavo Vono",
                DtBirth = new DateTime(1997, 1, 3),
                AddressDTO = new AddressDTO
                {
                    ZipCode = "17212-310",
                    Number = "123",
                    Complement = "My House"
                }
            };

            var result = await controller.PostPassenger(passengerDTO);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.Equal(badRequestResult.Value, "CPF já cadastrado");
        }
    }
}