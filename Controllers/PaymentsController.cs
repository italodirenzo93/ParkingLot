using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using VehiklParkingApi.Models;
using VehiklParkingApi.ViewModels;

namespace VehiklParkingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly VehiklParkingContext context;

        public PaymentsController(VehiklParkingContext context)
        {
            this.context = context;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> Post(int id, [FromBody] PaymentDto payment)
        {
            if (id != payment.TicketId)
                return BadRequest();

            var ticket = await context.Tickets.FindAsync(payment.TicketId);
            if (ticket == null)
                return NotFound();
            
            // Credit card transaction stuff goes here...

            // Delete the ticket. It has been paid for
            context.Tickets.Remove(ticket);
            await context.SaveChangesAsync();
            return Ok();    // Maybe return some kind of "reciept" here.
        }
    }
}