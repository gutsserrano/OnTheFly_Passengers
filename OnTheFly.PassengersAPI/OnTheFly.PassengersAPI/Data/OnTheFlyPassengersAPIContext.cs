using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace OnTheFly.PassengersAPI.Data
{
    public class OnTheFlyPassengersAPIContext : DbContext
    {
        public OnTheFlyPassengersAPIContext (DbContextOptions<OnTheFlyPassengersAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Passenger> Passenger { get; set; } = default!;
        public DbSet<Models.DeletedPassenger> DeletedPassanger { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Passenger>()
                .Ignore(p => p.Address);

            modelBuilder.Entity<DeletedPassenger>()
                .Ignore(p => p.Address);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<Passenger>())
            {
                if (entry.State == EntityState.Added)
                {
                    var passanger = entry.Entity;

                    var deletedPassanger = Set<DeletedPassenger>().FirstOrDefault(
                        p => p.Cpf.Replace("-", "").Replace(".", "") == passanger.Cpf.Replace("-", "").Replace(".", "")
                    );

                    if (deletedPassanger != null)
                        Set<DeletedPassenger>().Remove(deletedPassanger);
                }

                if (entry.State == EntityState.Deleted)
                {
                    var deletedPassanger = new DeletedPassenger
                    {
                        Cpf = entry.OriginalValues.GetValue<string>("Cpf"),
                        Name = entry.OriginalValues.GetValue<string>("Name"),
                        Gender = entry.OriginalValues.GetValue<char>("Gender"),
                        Phone = entry.OriginalValues.GetValue<string>("Phone"),
                        DtBirth = entry.OriginalValues.GetValue<DateTime>("DtBirth"),
                        DtRegister = entry.OriginalValues.GetValue<DateTime>("DtRegister"),
                        Restricted = entry.OriginalValues.GetValue<bool>("Restricted"),
                        AddressZipCode = entry.OriginalValues.GetValue<string>("AddressZipCode"),
                        AddressNumber = entry.OriginalValues.GetValue<string>("AddressNumber"),
                    };

                    Set<DeletedPassenger>().Add(deletedPassanger);
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
