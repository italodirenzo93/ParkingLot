using System;

namespace ParkingLot.Api.ViewModels
{
    public class InvoiceDto
    {
        public int TicketId { get; set; }
        public string Customer { get; set; }
        public DateTimeOffset IssuedOn { get; set; }
        public DateTimeOffset CurrentTime { get; } = DateTimeOffset.UtcNow;
        public string Rate { get; set; }
        public decimal BaseRate { get; set; }
        public decimal AmountOwed { get; set; }
    }
}