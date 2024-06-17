using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace OnTheFly.AddressAPI.Data
{
    public class OnTheFlyAddressAPIContext : DbContext
    {
        public OnTheFlyAddressAPIContext (DbContextOptions<OnTheFlyAddressAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Address> Address { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Address>()
                .HasKey(a => new { a.ZipCode, a.Number });
        }
    }
}
