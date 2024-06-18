using Microsoft.EntityFrameworkCore;
using Models;
using OnTheFly.AddressAPI.Data;
using OnTheFly.PassengersAPI.Controllers;
using OnTheFly.PassengersAPI.Data;

namespace OnTheFly.Tests
{
    public class DeletePassengerTest
    {
        private readonly DbContextOptions<OnTheFlyPassengersAPIContext> _options;

        public DeletePassengerTest()
        {
            _options = new DbContextOptionsBuilder<OnTheFlyPassengersAPIContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new OnTheFlyPassengersAPIContext(_options);

            context.Passenger.Add(new Passenger
            {
                Cpf = "484.512.608-70",
                Name = "Gustavo VONO",
                Gender = 'M',
                Phone = "14996866621",
                DtBirth = DateTime.Now,
                AddressZipCode = "172123333",
                AddressNumber = "65"
            });

            context.SaveChanges();
        }

        public PassengersController MakeController() => new PassengersController(
            new OnTheFlyPassengersAPIContext(_options),
            new Services.UpdatePassengerService(),
            new Services.CreatePassengerService(),
            new Services.GetPassengerService(),
            new Services.DeletePassengerService()
        );

        [Fact]
        public async Task Delete_ValidCpf_ShouldAddToDeletedPassangerTable()
        {
            var context = new OnTheFlyPassengersAPIContext(_options);
            var controller = MakeController();
            string cpf = "484.512.608-70";

            controller.DeletePassenger(cpf);

            Assert.Equal(0, await context.Passenger.CountAsync());
            Assert.Equal(1, await context.DeletedPassanger.CountAsync());
            Assert.Equal(cpf, (await context.DeletedPassanger.FirstOrDefaultAsync(p => p.Cpf == cpf)).Cpf);
        }
    }
}
