using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using VehiklParkingApi.Models;

namespace VehiklParkingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly VehiklParkingContext context;

        public TicketsController(VehiklParkingContext context)
        {
            this.context = context;
        }

        // GET api/tickets
        [HttpGet]
        public async Task<IEnumerable<Ticket>> Get()
        {
            return await context.Tickets.Include(t => t.RateLevel).ToListAsync();
        }

        // GET api/tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var ticket = await context.Tickets.Include(t => t.RateLevel).FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null)
                return NotFound();
            return Ok(ticket);
        }

        // POST api/tickets
        [HttpPost]
        public async Task<ActionResult> Post([Bind("Customer", "RateLevelId")] Ticket ticket)
        {
            await context.AddAsync(ticket);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(Post), ticket);
        }
    }
}
