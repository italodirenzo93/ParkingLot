using System.Collections.Generic;
using ParkingLot.Data.Models;

namespace ParkingLot.Api.Responses
{
    public class AllTicketsResponse
    {
        public int SpacesAvailable { get; set; }
        public int SpacesTaken { get; set; }
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
