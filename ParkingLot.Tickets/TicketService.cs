using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParkingLot.Data;
using ParkingLot.Data.Models;

namespace ParkingLot.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly ParkingLotDbContext _context;
        private readonly ParkingLotConfig _config;

        public TicketService(ParkingLotDbContext context, ParkingLotConfig config)
        {
            _context = context;
            _config = config;
        }

        public async Task<List<Ticket>> GetAll() =>
            await _context.Tickets.AsNoTracking().ToListAsync();

        public async Task<int> GetTotal() => await _context.Tickets.CountAsync();

        public async Task<Ticket> GetById(int id) => await _context.Tickets.AsNoTracking().Include(x => x.RateLevel)
            .FirstOrDefaultAsync(x => x.Id == id);

        /// <summary>
        /// Formula is (rate * stayDuration / rateTime) rounded to 2 decimal places.
        /// </summary>
        /// <param name="ticket">Ticket to calculate the amount owing for.</param>
        /// <returns>The monetary amount owed.</returns>
        public decimal GetAmountOwed(Ticket ticket)
        {
            if (ticket.RateLevel == null)
            {
                throw new ArgumentException("Ticket instance must have a rate level", nameof(ticket));
            }
            
            // All-day overage charges would be calculated based on whatever the lot's policy and definition of "All day" is
            // For now, we'll just assume they pay the flat rate for the duration of their stay
            if (ticket.RateLevel.Duration == null) return ticket.RateLevel.RateValue;

            // If this is a timed rate, calculate the amount the customer owes
            // Difference in time between when they pulled into the lot and now
            var lengthOfStay = DateTimeOffset.UtcNow - ticket.IssuedOn;

            // Calculate the final ticket price (rate * stayDuration / rateTime)
            var price = ticket.RateLevel!.RateValue *
                        (decimal) (lengthOfStay.TotalHours / ticket.RateLevel!.Duration!.Value.TotalHours);

            // Format to 2 decimal places
            return decimal.Round(price, 2);
        }

        public async Task<Ticket> IssueNewTicket(string customer, int rateLevelId)
        {
            // Deny entry if the garage is full
            var ticketCount = await _context.Tickets.CountAsync();
            if (ticketCount >= _config.MaxParkingSpaces)
                throw new LotFullException(_config.MaxParkingSpaces);

            // Give a ticket
            var ticket = new Ticket
            {
                Customer = customer,
                RateLevelId = rateLevelId
            };

            await _context.AddAsync(ticket);
            await _context.SaveChangesAsync();

            await _context.Entry(ticket).Reference(x => x.RateLevel).LoadAsync();

            return ticket;
        }

        public async Task PayTicket(int ticketId)
        {
            // Check ticket
            var ticket = await _context.FindAsync<Ticket>(ticketId);
            if (ticket == null)
                throw new TicketNotFoundException(ticketId);
            
            // Credit card transaction stuff goes here...

            // Delete the ticket. It has been paid for (opening up a space in the garage)
            _context.Remove(ticket);
            await _context.SaveChangesAsync();
        }
    }
}
