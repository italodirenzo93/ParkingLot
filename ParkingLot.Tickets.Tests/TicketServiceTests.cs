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
        private readonly ParkingLotDbContext _context;

        public TicketServiceTests()
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(nameof(TicketServiceTests))
                .Options;

            _context = new ParkingLotDbContext(options);
        }

        [Fact]
        public async Task ItIssuesATicketIfThereIsASpace()
        {
            // arrange
            var config = new ParkingLotConfig {MaxParkingSpaces = 3};
            var ticketService = new TicketService(_context, config);
            var rateLevel = new RateLevel
            {
                Name = "Test Rate",
                RateValue = 1.25M
            };
            
            var tickets = new[]
            {
                new Ticket {Customer = "Test Customer 1", RateLevel = rateLevel},
                new Ticket {Customer = "Test Customer 2", RateLevel = rateLevel}
            };

            await _context.Tickets.AddRangeAsync(tickets);
            await _context.SaveChangesAsync();
            
            // act
            var newTicket = await ticketService.IssueNewTicket("cust", rateLevel.Id);
            
            // assert
            Assert.NotNull(newTicket);
            Assert.Equal("cust", newTicket.Customer);
        }

        [Fact]
        public async Task ItRefusesEntryIfTheLotIsFull()
        {
            // arrange
            var config = new ParkingLotConfig {MaxParkingSpaces = 3};
            var ticketService = new TicketService(_context, config);
            var rateLevel = new RateLevel
            {
                Name = "Test Rate",
                RateValue = 1.25M
            };
            
            var tickets = new[]
            {
                new Ticket {Customer = "Test Customer 1", RateLevel = rateLevel},
                new Ticket {Customer = "Test Customer 2", RateLevel = rateLevel},
                new Ticket {Customer = "Test Customer 3", RateLevel = rateLevel}
            };

            await _context.Tickets.AddRangeAsync(tickets);
            await _context.SaveChangesAsync();
            
            // act/assert
            await Assert.ThrowsAsync<LotFullException>(async () => await ticketService.IssueNewTicket("Test Customer 4", 1));
        }
        
        [Theory]
        [MemberData(nameof(ItCalculatesTheCorrectOwingAmount_Data))]
        public async Task ItCalculatesTheCorrectOwingAmount(DateTimeOffset issuedOn, TimeSpan rateDuration, decimal rateValue, decimal expectedTotal)
        {
            // arrange
            var config = new ParkingLotConfig {MaxParkingSpaces = 3};
            var ticketService = new TicketService(_context, config);
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
            var amountOwing = ticketService.GetAmountOwed(ticket);
            
            // assert
            Assert.Equal(expectedTotal, amountOwing);
        }

        [Fact]
        public void ItThrowsArgumentException_IfTicketHasNoRateLevel()
        {
            // arrange
            var ticket = new Ticket
            {
                Id = 2,
                RateLevelId = 12,
                RateLevel = null
            };

            var service = new TicketService(_context, new ParkingLotConfig());
            
            // act/assert
            Assert.Throws<ArgumentException>(() => service.GetAmountOwed(ticket));
        }

        public static IEnumerable<object[]> ItCalculatesTheCorrectOwingAmount_Data => new[]
        {
            new object[] {DateTimeOffset.UtcNow.AddHours(-6), TimeSpan.FromHours(3), 1.50M, 3M},
            new object[] {DateTimeOffset.UtcNow.AddHours(-4), TimeSpan.FromHours(12), 1.25M, 0.42M}
        };
    }
}
