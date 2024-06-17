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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Passenger>()
                .Ignore(p => p.Address);
        }
    }
}
