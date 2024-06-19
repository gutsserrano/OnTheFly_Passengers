using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using OnTheFly.PassengersAPI.Controllers;
using OnTheFly.PassengersAPI.Data;
using Services;

namespace OnTheFly.Tests
{
    public class UpdatePassengerTest
    {
        private readonly DbContextOptions<OnTheFlyPassengersAPIContext> _options;
        private readonly CreatePassengerService _createPassengerService;
        private readonly GetPassengerService _getPassengerService;
        private readonly UpdatePassengerService _updatePassengerService;
        private readonly DeletePassengerService _deletePassengerService;

        public UpdatePassengerTest()
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
                Gender = 'M',
                Phone = "14999999999",
                AddressNumber = "123",
                AddressZipCode = "17212-310",
                Restricted = false
            });

            context.SaveChanges();
        }

        [Fact]
        public async Task UpdateTest_ReturnsOk()
        {
            using (var context = new OnTheFlyPassengersAPIContext(_options))
            {
                var passengerDTO = new PassengerUpdateDTO
                {
                    Cpf = "745.933.040-08",
                    Name = "Gustavo Vono",
                    DtBirth = new DateTime(1997, 1, 3),
                    Gender = 'M',
                    Phone = "14999999999",
                    Restricted = true
                };

                var controller = new PassengersController(context, _updatePassengerService, _createPassengerService, _getPassengerService, _deletePassengerService);
                var result = await controller.PutPassenger("745.933.040-08", passengerDTO);
                var okObject = result.Result as OkObjectResult;
                var updatedPassenger = Assert.IsAssignableFrom<Passenger>(okObject.Value);
                Assert.True(updatedPassenger.Restricted);
            }
        }

        [Fact]
        public async Task UpdateTest_InvalidCpf_ReturnsBadRequest()
        {
            using (var context = new OnTheFlyPassengersAPIContext(_options))
            {
                var passengerDTO = new PassengerUpdateDTO
                {
                    Cpf = "bolacha",
                    Name = "Gustavo Vono",
                    DtBirth = new DateTime(1997, 1, 3),
                    Gender = 'M',
                    Phone = "14999999999",
                    Restricted = true
                };

                var controller = new PassengersController(context, _updatePassengerService, _createPassengerService, _getPassengerService, _deletePassengerService);
                var result = await controller.PutPassenger("745.933.040-08", passengerDTO);
                Assert.IsType<BadRequestResult>(result.Result);
            }
        }

        [Fact]
        public async Task UpdateTest_Cpf_ReturnsNotFound()
        {
            using (var context = new OnTheFlyPassengersAPIContext(_options))
            {
                var passengerDTO = new PassengerUpdateDTO
                {
                    Cpf = "029.894.940-73",
                    Name = "Gustavo Vono",
                    DtBirth = new DateTime(1997, 1, 3),
                    Gender = 'M',
                    Phone = "14999999999",
                    Restricted = true
                };

                var controller = new PassengersController(context, _updatePassengerService, _createPassengerService, _getPassengerService, _deletePassengerService);
                var result = await controller.PutPassenger("029.894.940-73", passengerDTO);
                var notFoundResult = result.Result as NotFoundObjectResult;

                Assert.IsType<NotFoundObjectResult>(result.Result);
                Assert.Equal("Passageiro não encontrado.", notFoundResult.Value);
            }
        }
    }
}