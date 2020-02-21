using System;

namespace ParkingLot.Web.Models
{
    public class Invoice
    {
        public int TicketId { get; set; }
        public string Customer { get; set; }
        public DateTimeOffset IssuedOn { get; set; }
        public DateTimeOffset CurrentTime { get; set; }
        public string Rate { get; set; }
        public decimal BaseRate { get; set; }
        public decimal AmountOwed { get; set; }
    }
}
