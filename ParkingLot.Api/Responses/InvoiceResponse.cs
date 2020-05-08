using System;

namespace ParkingLot.Api.Responses
{
    public class InvoiceResponse
    {
        public int TicketId { get; set; }
        public string Customer { get; set; } = string.Empty;
        public DateTimeOffset IssuedOn { get; set; }
        public DateTimeOffset CurrentTime { get; } = DateTimeOffset.UtcNow;
        public string Rate { get; set; } = string.Empty;
        public decimal BaseRate { get; set; }
        public decimal AmountOwed { get; set; }
    }
}
