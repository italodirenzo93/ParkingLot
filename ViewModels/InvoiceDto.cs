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
        public decimal AmountOwed { get; set; }
    }
}