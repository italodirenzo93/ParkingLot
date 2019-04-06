
namespace VehiklParkingApi.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public System.DateTimeOffset IssuedOn { get; set; } = System.DateTimeOffset.Now;
        public int RateLevelId { get; set; }
        public RateLevel RateLevel { get; set; }
    }
}