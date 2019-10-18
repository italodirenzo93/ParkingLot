using System;
using System.Collections.Generic;
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
        
        [Theory]
        [MemberData(nameof(ItCalculatesTheCorrectOwingAmount_Data))]
        public async Task ItCalculatesTheCorrectOwingAmount(DateTimeOffset issuedOn, TimeSpan rateDuration, decimal rateValue, decimal expectedTotal)
        {
            // arrange
            var rateLevel = new RateLevel
            {
                Name = rateDuration.ToString(),
                Duration = rateDuration,
                RateValue = rateValue
            };

            var ticket = new Ticket
            {
                Customer = "Test Customer",
                RateLevel = rateLevel,
                IssuedOn = issuedOn
            };

            await _context.AddAsync(ticket);
            await _context.SaveChangesAsync();
            
            // act
            var amountOwing = _ticketService.GetAmountOwed(ticket);
            
            // assert
            Assert.Equal(expectedTotal, amountOwing);
        }

        public static IEnumerable<object[]> ItCalculatesTheCorrectOwingAmount_Data => new[]
        {
            new object[] {DateTimeOffset.UtcNow.AddHours(-6), TimeSpan.FromHours(3), 1.50M, 3M}
        };
    }
}