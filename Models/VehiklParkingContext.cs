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
            RateLevels.AddRange(new RateLevel[] {
                new RateLevel { Id = 1, Name = "1hr", Duration = TimeSpan.FromHours(1), RateValue = 3.00M },
                new RateLevel { Id = 2, Name = "3hr", Duration = TimeSpan.FromHours(3), RateValue = 4.50M },
                new RateLevel { Id = 3, Name = "6hr", Duration = TimeSpan.FromHours(6), RateValue = 6.75M },
                new RateLevel { Id = 4, Name = "ALL DAY", Duration = null, RateValue = 10.125M },
            });
            this.SaveChanges();
        }
    }
}