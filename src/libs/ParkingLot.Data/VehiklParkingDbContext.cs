using System;
using Microsoft.EntityFrameworkCore;
using ParkingLot.Data.Models;

namespace ParkingLot.Data
{
    public class VehiklParkingDbContext : DbContext
    {
        public VehiklParkingDbContext(DbContextOptions options)
            : base(options)
        {   
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<RateLevel> RateLevels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Store timespan as milliseconds
            modelBuilder.Entity<RateLevel>()
                .Property(e => e.Duration)
                .HasConversion(v => v.HasValue ? (double?) v.Value.TotalMilliseconds : null,
                    v => v.HasValue ? (TimeSpan?) TimeSpan.FromMilliseconds(v.Value) : null);
        }
    }
}