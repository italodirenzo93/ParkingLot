using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ParkingLot.Api.ViewModels;
using ParkingLot.Data;
using ParkingLot.Data.Models;
using ParkingLot.Tickets;

namespace ParkingLot.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly VehiklParkingDbContext _context;
        private readonly ParkingLotConfig _config;
        private readonly ITicketService _ticketService;

        public TicketsController(VehiklParkingDbContext context, ParkingLotConfig config, ITicketService ticketService)
        {
            _context = context;
            _config = config;
            _ticketService = ticketService;
        }

        // GET api/tickets
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var tickets = await _context.Tickets.Include(t => t.RateLevel).ToListAsync();
            return Ok(new { spacesTaken = tickets.Count, spacesAvailable = _config.MaxParkingSpaces - tickets.Count, tickets });
        }

        // GET api/tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var ticket = await _context.Tickets.Include(t => t.RateLevel).FirstOrDefaultAsync(t => t.Id == id);
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
        public async Task<ActionResult> Post([Bind("Customer", "RateLevelId")] Ticket ticket)
        {
            // Check for a valid rate level
            ticket.RateLevel = await _context.FindAsync<RateLevel>(ticket.RateLevelId);
            if (ticket.RateLevel == null)
                return BadRequest(new { status = 400, message = "Invalid rate level chosen. Please specify a valid rate level." });

            // Check if the garage is full
            var ticketCount = await _context.Tickets.CountAsync();

            // Deny entry if the garage is full
            if (ticketCount >= _config.MaxParkingSpaces)
                return StatusCode(429, new { status = 429, message = "Parking Garage is full." }); // Too Many Requests (garage is full)

            // Give a ticket
            await _context.AddAsync(ticket);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Post), new { ticket.Id, ticket.Customer, ticket.IssuedOn, rate = ticket.RateLevel.Name });
        }
    }
}
