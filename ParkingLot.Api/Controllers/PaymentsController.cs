using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkingLot.Api.ViewModels;
using ParkingLot.Tickets;

namespace ParkingLot.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly ParkingLotConfig _config;
        private readonly ITicketService _ticketService;

        public PaymentsController(ParkingLotConfig config, ITicketService ticketService)
        {
            _config = config;
            _ticketService = ticketService;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> Post(int id, [FromBody] PaymentDto payment)
        {
            // Check that the payment is addressed to this ticket's endpoint
            if (id != payment.TicketId)
                return BadRequest(new { status = 400, message = "Incorrect ticket number specified." });

            try
            {
                await _ticketService.PayTicket(id);
            }
            catch (TicketNotFoundException ex)
            {
                return NotFound(new {status = 404, message = ex.Message});
            }

            // Return response
            var spacesTaken = await _ticketService.GetTotal();
            return Ok(new { message = "Thank you!", spacesTaken, spacesAvailable = _config.MaxParkingSpaces - spacesTaken });    // Maybe return some kind of "reciept" here.
        }
    }
}