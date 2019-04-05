using System.ComponentModel.DataAnnotations;

namespace VehiklParkingApi.ViewModels
{
    public class PaymentDto
    {
        public int TicketId { get; set; }

        [Required]
        public string CreditCardNumber { get; set; }
    }
}