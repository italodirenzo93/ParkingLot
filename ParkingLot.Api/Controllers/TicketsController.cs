using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkingLot.Api.Requests;
using ParkingLot.Api.Responses;
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
            var tickets = await _ticketService.GetAll();
            return Ok(new AllTicketsResponse
            {
                SpacesTaken = tickets.Count,
                SpacesAvailable = _config.MaxParkingSpaces - tickets.Count,
                Tickets = tickets
            });
        }

        // GET api/tickets/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var ticket = await _ticketService.GetById(id);
            if (ticket == null)
                return NotFound();

            // Build the invoice model
            var invoice = new InvoiceResponse
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
            
            return CreatedAtAction(nameof(Post), new CreatedTicketResponse
            {
                Id = ticket.Id,
                Customer = ticket.Customer,
                IssuedOn = ticket.IssuedOn,
                Rate = ticket.RateLevel.Name
            });
        }
    }
}
