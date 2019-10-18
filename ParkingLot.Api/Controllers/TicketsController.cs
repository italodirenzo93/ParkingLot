using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingLot.Api.ViewModels;
using ParkingLot.Data.Models;
using ParkingLot.Tickets;

namespace ParkingLot.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ParkingLotConfig _config;
        private readonly ITicketService _ticketService;

        public TicketsController(ParkingLotConfig config, ITicketService ticketService)
        {
            _config = config;
            _ticketService = ticketService;
        }

        // GET api/tickets
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tickets = await _ticketService.Queryable.Include(t => t.RateLevel).ToListAsync();
            return Ok(new { spacesTaken = tickets.Count, spacesAvailable = _config.MaxParkingSpaces - tickets.Count, tickets });
        }

        // GET api/tickets/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var ticket = await _ticketService.Queryable.Include(t => t.RateLevel).FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null)
                return NotFound();

            // Build the invoice model
            var invoice = new InvoiceDto
            {
                TicketId = ticket.Id,
                Customer = ticket.Customer,
                IssuedOn = ticket.IssuedOn,
                Rate = ticket.RateLevel.Name,
                BaseRate = ticket.RateLevel.RateValue,
                AmountOwed = _ticketService.GetAmountOwed(ticket)
            };

            return Ok(invoice);
        }

        // POST api/tickets
        [HttpPost]
        public async Task<IActionResult> Post([Bind("Customer", "RateLevelId")] Ticket ticket)
        {
            try
            {
                ticket = await _ticketService.IssueNewTicket(ticket.Customer, ticket.RateLevelId);
            }
            catch (LotFullException ex)
            {
                return StatusCode(429, new { status = 429, message = ex.Message }); // Too Many Requests (garage is full)
            }
            
            return CreatedAtAction(nameof(Post), new { ticket.Id, ticket.Customer, ticket.IssuedOn, rate = ticket.RateLevel.Name });
        }
    }
}
