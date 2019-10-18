using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParkingLot.Data;
using ParkingLot.Data.Models;
using Xunit;

namespace ParkingLot.Tickets.Tests
{
    public class TicketServiceTests
    {
        private readonly VehiklParkingDbContext _context;
        private readonly TicketService _ticketService;

        public TicketServiceTests()
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(nameof(TicketServiceTests))
                .Options;

            _context = new VehiklParkingDbContext(options);
            _ticketService = new TicketService(_context);
        }
        
        [Fact]
        public async Task ItCalculatesTheCorrectOwingAmount()
        {
            // arrange
            var rateLevel = new RateLevel
            {
                Name = "3 Hours",
                Duration = TimeSpan.FromHours(3),
                RateValue = 1.50M
            };

            var ticket = new Ticket
            {
                Customer = "Test Customer",
                RateLevel = rateLevel,
                IssuedOn = DateTimeOffset.UtcNow.AddHours(-6)
            };

            await _context.AddAsync(ticket);
            await _context.SaveChangesAsync();
            
            // act
            var amountOwing = _ticketService.GetAmountOwed(ticket);
            
            // assert
            Assert.Equal(3M, amountOwing);
        }
    }
}