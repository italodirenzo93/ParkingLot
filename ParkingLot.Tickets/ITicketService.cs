using System.Threading.Tasks;
using ParkingLot.Data.Models;

namespace ParkingLot.Tickets
{
    public interface ITicketService
    {
        Task<Ticket> GetById(int id);
        decimal GetAmountOwed(Ticket ticket);
    }
}