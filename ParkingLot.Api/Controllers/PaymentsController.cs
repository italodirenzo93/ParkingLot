using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkingLot.Api.Requests;
using ParkingLot.Api.Responses;
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
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PaymentResponse), 200)]
        public async Task<IActionResult> Post(int id, [FromBody] PaymentRequest payment)
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

            // Maybe return some kind of "receipt" here.
            var spacesTaken = await _ticketService.GetTotal();
            return Ok(new PaymentResponse
            {
                Message = "Thank you!",
                SpacesTaken = spacesTaken,
                SpacesAvailable = _config.MaxParkingSpaces - spacesTaken
            });
        }
    }
}
