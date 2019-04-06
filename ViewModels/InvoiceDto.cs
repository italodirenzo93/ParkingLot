namespace VehiklParkingApi.ViewModels
{
    public class InvoiceDto
    {
        public int TicketId { get; set; }
        public string Customer { get; set; }
        public System.DateTimeOffset IssuedOn { get; set; }
        public System.DateTimeOffset CurrentTime { get; } = System.DateTimeOffset.Now;
        public string Rate { get; set; }
        public decimal BaseRate { get; set; }

        private decimal amountOwed;
        public decimal AmountOwed
        {
            // Format to 2 decimal places in the getter in case we decide to change the precision later
            get { return decimal.Round(this.amountOwed, 2); }
            set { this.amountOwed = value; }
        }
    }
}