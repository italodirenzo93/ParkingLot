namespace ParkingLot.Api.Responses
{
    public class PaymentResponse
    {
        public string Message { get; set; } = string.Empty;
        public int SpacesTaken { get; set; }
        public int SpacesAvailable { get; set; }
    }
}
