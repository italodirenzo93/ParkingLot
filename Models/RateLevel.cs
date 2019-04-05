namespace VehiklParkingApi.Models
{
    public class RateLevel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public System.TimeSpan? Duration { get; set; }
        public decimal RateValue { get; set; }
    }
}