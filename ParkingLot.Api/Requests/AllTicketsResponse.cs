using System.Collections.Generic;
using ParkingLot.Data.Models;

namespace ParkingLot.Api.Requests
{
    public class AllTicketsResponse
    {
        public int SpacesAvailable { get; set; }
        public int SpacesTaken { get; set; }
        public IEnumerable<Ticket> Tickets { get; set; }
    }
}