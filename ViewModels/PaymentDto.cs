using System.ComponentModel.DataAnnotations;

namespace VehiklParkingApi.ViewModels
{
    public class PaymentDto
    {
        public int TicketId { get; set; }

        [Required, CreditCard]
        public string CreditCard { get; set; }
    }
}