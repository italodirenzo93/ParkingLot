using System;
using Microsoft.EntityFrameworkCore;

namespace VehiklParkingApi.Models
{
    public class VehiklParkingContext : DbContext
    {
        public VehiklParkingContext(DbContextOptions options)
            : base(options)
        {   
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<RateLevel> RateLevels { get; set; }

        public void Seed()
        {
            // Define the different rate levels in a separate table (allows them to be changed without needing to modify program code)
            RateLevels.AddRange(new RateLevel[] {
                new RateLevel { Id = 1, Name = "1hr", Duration = TimeSpan.FromHours(1), RateValue = 3.00M },
                new RateLevel { Id = 2, Name = "3hr", Duration = TimeSpan.FromHours(3), RateValue = 4.50M },
                new RateLevel { Id = 3, Name = "6hr", Duration = TimeSpan.FromHours(6), RateValue = 6.75M },
                new RateLevel { Id = 4, Name = "ALL DAY", Duration = null, RateValue = 10.125M },
            });

            // Add some test tickets
            var now = DateTimeOffset.Now;
            Tickets.AddRange(new Ticket[] {
                new Ticket { Customer = "Italo Di Renzo", RateLevelId = 3, IssuedOn = now.AddHours(-10) },
                new Ticket { Customer = "Tim Berners-Lee", RateLevelId = 1, IssuedOn = now.AddHours(-4) },
                new Ticket { Customer = "Leon S. Kennedy", RateLevelId = 4, IssuedOn = now.AddHours(-13) },
                new Ticket { Customer = "Gordon Freeman", RateLevelId = 1, IssuedOn = now.AddHours(-2) },
            });

            this.SaveChanges();
        }
    }
}