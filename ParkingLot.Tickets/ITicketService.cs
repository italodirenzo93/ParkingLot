using System.Collections.Generic;
using System.Threading.Tasks;
using ParkingLot.Data.Models;

namespace ParkingLot.Tickets
{
    public interface ITicketService
    {
        Task<List<Ticket>> GetAll();
        Task<int> GetTotal();
        Task<Ticket> GetById(int id);
        decimal GetAmountOwed(Ticket ticket);
        Task<Ticket> IssueNewTicket(string customer, int rateLevelId);
        Task PayTicket(int ticketId);
    }
}