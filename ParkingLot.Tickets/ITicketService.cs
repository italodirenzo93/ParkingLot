using System.Linq;
using System.Threading.Tasks;
using ParkingLot.Data.Models;

namespace ParkingLot.Tickets
{
    public interface ITicketService
    {
        IQueryable<Ticket> Queryable { get; }
        decimal GetAmountOwed(Ticket ticket);
        Task<Ticket> IssueNewTicket(string customer, int rateLevelId);
    }
}