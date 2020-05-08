using System.ComponentModel.DataAnnotations;

namespace ParkingLot.Api.Requests
{
    public class PaymentRequest
    {
        public int TicketId { get; set; }

        [Required, CreditCard] public string CreditCard { get; set; } = string.Empty;
    }
} 
