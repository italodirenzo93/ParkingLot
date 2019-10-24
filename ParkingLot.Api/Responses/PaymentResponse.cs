namespace ParkingLot.Api.Responses
{
    public class PaymentResponse
    {
        public string Message { get; set; }
        public int SpacesTaken { get; set; }
        public int SpacesAvailable { get; set; }
    }
}