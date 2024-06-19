using Microsoft.EntityFrameworkCore;
using Models;
using OnTheFly.PassengersAPI.Controllers;
using OnTheFly.PassengersAPI.Data;
using Services;

namespace OnTheFly.Tests
{
    public class UpdatePassengerTest
    {
        private DbContextOptions<OnTheFlyPassengersAPIContext> _options = new DbContextOptionsBuilder<OnTheFlyPassengersAPIContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

        [Fact]
        public void UpdateTest()
        {
            using (var context = new OnTheFlyPassengersAPIContext(_options))
            {
                var passenger = new Passenger
                {
                    Cpf = "22222",
                    Name = "Test",
                    Gender = 'M',
                    Phone = "123456789",
                    DtBirth = DateTime.Now,
                    DtRegister = DateTime.Now,
                    Restricted = false,
                    Address = new Address
                    {
                        ZipCode = "123456789",
                        Street = "Test",
                        Number = "123",
                        Complement = "Test",
                        City = "Test",
                        State = "Test"
                    },
                    AddressZipCode = "12345678",
                    AddressNumber = "123"
                };
                context.Passenger.Add(passenger);
                context.SaveChanges();
            }

            using (var context = new OnTheFlyPassengersAPIContext(_options))
            {
                var controller = new PassengersController(context, new UpdatePassengerService(), new CreatePassengerService(), new GetPassengerService(), new DeletePassengerService());
                var passenger = controller.GetPassenger("22222").Result.Value;
                passenger.Name = "Updated2";

                PassengerUpdateDTO pDto = new()
                {
                    Cpf = passenger.Cpf,
                    Name = passenger.Name,
                    Gender = passenger.Gender,
                    Phone = passenger.Phone,
                    DtBirth = passenger.DtBirth,
                    Restricted = passenger.Restricted
                };

                controller.PutPassenger(passenger.Cpf, pDto);

                var result = controller.GetPassenger(passenger.Cpf).Result;
                Assert.Equal(result.Value.Name, "Updated2");
            }

        }
    }
}

