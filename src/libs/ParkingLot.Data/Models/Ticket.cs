
using System;

namespace ParkingLot.Data.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public DateTimeOffset IssuedOn { get; set; } = DateTimeOffset.UtcNow;
        public int RateLevelId { get; set; }
        public RateLevel RateLevel { get; set; }
    }
}