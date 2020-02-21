using System;

namespace ParkingLot.Web.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public DateTimeOffset IssuedOn { get; set; }
        public int RateLevelId { get; set; }
        public RateLevel RateLevel { get; set; }
    }

    public class TicketsResponse
    {
        public int SpacesAvailable { get; set; }
        public int SpacesTaken { get; set; }
        public Ticket[] Tickets { get; set; }
    }
}
