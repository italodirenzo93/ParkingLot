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
    public class PaymentsController : ControllerBase
    {
        private readonly VehiklParkingDbContext _context;
        private readonly ParkingLotConfig _config;

        public PaymentsController(VehiklParkingDbContext context, ParkingLotConfig config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> Post(int id, [FromBody] PaymentDto payment)
        {
            // Check ticket
            var ticket = await _context.FindAsync<Ticket>(payment.TicketId);
            if (ticket == null)
                return NotFound();
            
            // Check that the payment is addressed to this ticket's endpoint
            if (id != payment.TicketId)
                return BadRequest(new { status = 400, message = "Incorrect ticket number specified." });
            
            // Credit card transaction stuff goes here...

            // Delete the ticket. It has been paid for (opening up a space in the garage)
            _context.Remove(ticket);
            await _context.SaveChangesAsync();

            // Return response
            var spacesTaken = await _context.Tickets.CountAsync();
            return Ok(new { message = "Thank you!", spacesTaken, spacesAvailable = _config.MaxParkingSpaces - spacesTaken });    // Maybe return some kind of "reciept" here.
        }
    }
}