using System;

namespace ParkingLot.Data.Models
{
    public class RateLevel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public TimeSpan? Duration { get; set; }
        public decimal RateValue { get; set; }
    }
}
