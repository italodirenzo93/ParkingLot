using System;
using System.Threading.Tasks;
using ParkingLot.Data;
using ParkingLot.Data.Models;

namespace ParkingLot.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly VehiklParkingDbContext _context;

        public TicketService(VehiklParkingDbContext context)
        {
            _context = context;
        }

        public async Task<Ticket> GetById(int id) => await _context.FindAsync<Ticket>(id);

        public decimal GetAmountOwed(Ticket ticket)
        {
            // All-day overage charges would be calculated based on whatever the lot's policy and definition of "All day" is
            // For now, we'll just assume they pay the flat rate for the duration of their stay
            if (!ticket.RateLevel.Duration.HasValue) return ticket.RateLevel.RateValue;
            
            // If this is a timed rate, calculate the amount the customer owes
            // Difference in time between when they pulled into the lot and now
            var lengthOfStay = DateTimeOffset.UtcNow - ticket.IssuedOn;

            // Calculate the final ticket price (rate * stayDuration / rateTime)
            var price = ticket.RateLevel.RateValue * (decimal)(lengthOfStay.TotalHours / ticket.RateLevel.Duration.Value.TotalHours);
            
            // Format to 2 decimal places
            return decimal.Round(price, 2);
        }
    }
}