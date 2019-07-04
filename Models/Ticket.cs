
using System;

namespace VehiklParkingApi.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public DateTimeOffset IssuedOn { get; set; } = DateTimeOffset.Now;
        public int RateLevelId { get; set; }
        public RateLevel RateLevel { get; set; }
    }
}