using System;

namespace ParkingLot.Api.Responses
{
    public class CreatedTicketResponse
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public DateTimeOffset? IssuedOn { get; set; }
        public string Rate { get; set; }
    }
}