namespace ParkingLot.Web.Models
{
    public class PayTicketResponse
    {
        public string Message { get; set; }
        
        public int SpacesTaken { get; set; }
        
        public int SpacesAvailable { get; set; }
    }
}
