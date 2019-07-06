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

        private decimal _amountOwed;
        public decimal AmountOwed
        {
            // Format to 2 decimal places in the getter in case we decide to change the precision later
            get => decimal.Round(_amountOwed, 2);
            set => _amountOwed = value;
        }
    }
}