using Microsoft.EntityFrameworkCore;
using Models;
using OnTheFly.PassengersAPI.Controllers;
using OnTheFly.PassengersAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTheFly.Tests
{
    public class UpdatePassengerTest
    {
        private DbContextOptions<OnTheFlyPassengersAPIContext> _options;

        private void InitializeDataBase()
        {
            _options = new DbContextOptionsBuilder<OnTheFlyPassengersAPIContext>()
                .UseInMemoryDatabase(databaseName: "OnTheFly.DBPassenger")
                .Options;

            using(var context = new OnTheFlyPassengersAPIContext(_options))
            {
                context.Passenger.Add(new Passenger
                {
                    Cpf = "0987654",
                    Name = "Test",
                    Gender = 'M',
                    Phone = "123456789",
                    DtBirth = DateTime.Now,
                    DtRegister = DateTime.Now,
                    Restricted = false,
                    Address = new Address
                    {
                        ZipCode = "12345678",
                        Street = "Test",
                        Number = "123",
                        Complement = "Test",
                        City = "Test",
                        State = "Test"
                    },
                    AddressZipCode = "12345678",
                    AddressNumber = "123"
                });

                context.SaveChanges();
            }
        }

        [Fact]
        public void GetAllTest()
        {
            InitializeDataBase();

            using (var context = new OnTheFlyPassengersAPIContext(_options))
            {
                var controller = new PassengersController(context);
                var result = controller.GetPassenger().Result;

                Assert.Equal(result.Value.Count(), 1);
            }
        }

        [Fact]
        public void UpdateTest()
        {
            //InitializeDataBase();

            using (var context = new OnTheFlyPassengersAPIContext(_options))
            {
                var controller = new PassengersController(context);
                var passenger = controller.GetPassenger("0987654").Result.Value;
                //var passenger = context.Passenger.First();
                passenger.Name = "Updated2";

                controller.PutPassenger(passenger.Cpf, passenger);

                var result = controller.GetPassenger(passenger.Cpf).Result;

                Assert.Equal(result.Value.Name, "Updated2");
            }
        }
    }
}
