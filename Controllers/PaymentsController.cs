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
    public class PaymentsController : ControllerBase
    {
        private readonly VehiklParkingContext context;
        private readonly IConfiguration config;

        public PaymentsController(VehiklParkingContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> Post(int id, [FromBody] PaymentDto payment)
        {
            // Check ticket
            var ticket = await context.FindAsync<Ticket>(payment.TicketId);
            if (ticket == null)
                return NotFound();
            
            // Check that the payment is addressed to this ticket's endpoint
            if (id != payment.TicketId)
                return BadRequest(new { status = 400, message = "Incorrect ticket number specified." });
            
            // Credit card transaction stuff goes here...

            // Delete the ticket. It has been paid for (opening up a space in the garage)
            context.Remove(ticket);
            await context.SaveChangesAsync();

            // Return response
            var spacesTaken = await context.Tickets.CountAsync();
            var maxSpaces = config.GetValue<int>("MaxParkingSpaces");
            return Ok(new { message = "Thank you!", spacesTaken, spacesAvailable = maxSpaces - spacesTaken });    // Maybe return some kind of "reciept" here.
        }
    }
}