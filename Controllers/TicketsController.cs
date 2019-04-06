using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VehiklParkingApi.Models;
using VehiklParkingApi.ViewModels;

namespace VehiklParkingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly VehiklParkingContext context;
        private readonly IConfiguration config;

        public TicketsController(VehiklParkingContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }

        // GET api/tickets
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var tickets = await context.Tickets.Include(t => t.RateLevel).ToListAsync();
            var capacity = config.GetValue<int>("MaxParkingSpaces");
            return Ok(new { spacesTaken = tickets.Count, spacesAvailable = capacity - tickets.Count, tickets });
        }

        // GET api/tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var ticket = await context.Tickets.Include(t => t.RateLevel).FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null)
                return NotFound();

            // Build the invoice model
            var invoice = new InvoiceDto {
                TicketId = ticket.Id,
                Customer = ticket.Customer,
                IssuedOn = ticket.IssuedOn,
                Rate = ticket.RateLevel.Name,
                BaseRate = ticket.RateLevel.RateValue
            };

            // If this is a timed rate, calculate the amount the customer owes
            if (ticket.RateLevel.Duration.HasValue)
            {
                // Difference in time between when they pulled into the lot and now
                var lengthOfStay = (DateTimeOffset.Now - ticket.IssuedOn);

                // Calculate the final ticket price (rate * stayDuration / rateTime)
                invoice.AmountOwed = ticket.RateLevel.RateValue * (decimal)(lengthOfStay.TotalHours / ticket.RateLevel.Duration.Value.TotalHours);
            }
            // Flat rate
            else
            {
                // All-day overage charges would be calculated based on whatever the lot's policy and definition of "All day" is
                // For now, we'll just assume they pay the flat rate for the duration of their stay
                invoice.AmountOwed = ticket.RateLevel.RateValue;
            }
            
            return Ok(invoice);
        }

        // POST api/tickets
        [HttpPost]
        public async Task<ActionResult> Post([Bind("Customer", "RateLevelId")] Ticket ticket)
        {
            // Check for a valid rate level
            ticket.RateLevel = await context.FindAsync<RateLevel>(ticket.RateLevelId);
            if (ticket.RateLevel == null)
                return BadRequest(new { status = 400, message = "Invalid rate level chosen. Please specify a valid rate level." });

            // Check if the garage is full
            var ticketCount = await context.Tickets.CountAsync();
            var maxSpaces = config.GetValue<int>("MaxParkingSpaces");

            // Deny entry if the garage is full
            if (ticketCount >= maxSpaces)
                return StatusCode(429, new { status = 429, message = "Parking Garage is full." }); // Too Many Requests (garage is full)

            // Give a ticket
            await context.AddAsync(ticket);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(Post), new { ticket.Id, ticket.Customer, ticket.IssuedOn, rate = ticket.RateLevel.Name });
        }
    }
}
