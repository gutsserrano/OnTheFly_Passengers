using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using OnTheFly.PassengersAPI.Controllers;
using OnTheFly.PassengersAPI.Data;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTheFly.Tests
{
    public class GetPassengerTest
    {
        private readonly DbContextOptions<OnTheFlyPassengersAPIContext> _options;
        private readonly CreatePassengerService _createPassengerService;
        private readonly GetPassengerService _getPassengerService;
        private readonly UpdatePassengerService _updatePassengerService;
        private readonly DeletePassengerService _deletePassengerService;

        public GetPassengerTest()
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
                AddressNumber = "12",
                AddressZipCode = "14811-450",
                Restricted = false
            });

            context.Passenger.Add(new Passenger
            {
                Cpf = "263.938.660-39",
                Name = "Cleiton",
                DtBirth = new DateTime(1992, 1, 3),
                DtRegister = DateTime.Now,
                Gender = 'M',
                Phone = "14999999999",
                AddressNumber = "12",
                AddressZipCode = "14811-450",
                Restricted = false
            });

            context.SaveChanges();
        }

        [Fact]
        public async Task GetAll()
        {
            var context = new OnTheFlyPassengersAPIContext(_options);
            var controller = new PassengersController( context, _updatePassengerService, _createPassengerService, _getPassengerService, _deletePassengerService);
            var result = await controller.GetPassenger();
            var passengers = result.Value;
            Assert.Equal(2, passengers.Count());
        }

        [Fact]
        public async void GetByCpf_PassengerExists()
        {
            var context = new OnTheFlyPassengersAPIContext(_options);
            var controller = new PassengersController(context, _updatePassengerService, _createPassengerService, _getPassengerService, _deletePassengerService);
            var result = await controller.GetPassenger("263.938.660-39");
            var passenger = (Passenger) result.Value;
            Assert.True(passenger.Cpf == "263.938.660-39");
        }

        [Fact]
        public async void GetByCpf_PassengerDeleted()
        {
            var context = new OnTheFlyPassengersAPIContext(_options);
            var controller = new PassengersController(context, _updatePassengerService, _createPassengerService, _getPassengerService, _deletePassengerService);
            await controller.DeletePassenger("745.933.040-08");
            var result = await controller.GetPassenger("745.933.040-08", true);
            var passenger = (DeletedPassenger)result.Value;
            Assert.True(passenger.Cpf == "745.933.040-08");
        }

        [Fact]
        public async void GetByCpf_PassengerNotFound()
        {
            var context = new OnTheFlyPassengersAPIContext(_options);
            var controller = new PassengersController(context, _updatePassengerService, _createPassengerService, _getPassengerService, _deletePassengerService);
            var result = await controller.GetPassenger("363.938.660-39");
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
