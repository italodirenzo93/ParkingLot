using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ParkingLot.Api.Controllers;
using ParkingLot.Api.Requests;
using ParkingLot.Api.Responses;
using ParkingLot.Tickets;
using Xunit;

namespace ParkingLot.Api.Tests
{
    public class PaymentsControllerTests
    {
        private readonly ParkingLotConfig _config;
        private readonly Mock<ITicketService> _mockTicketService;
        private readonly PaymentsController _controller;

        public PaymentsControllerTests()
        {
            _config = new ParkingLotConfig {MaxParkingSpaces = 5};
            _mockTicketService = new Mock<ITicketService>();
            _controller = new PaymentsController(_config, _mockTicketService.Object);
        }

        [Fact]
        public async Task PaymentWithDifferingTicketIdReturnsBadRequest()
        {
            // arrange
            var paymentRequest = new PaymentRequest
            {
                TicketId = 33,
                CreditCard = "1111 1111 1111 1111"
            };
            
            // act
            var result = await _controller.Post(12, paymentRequest);
            
            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PaymentWithNonExistentTicketReturnsNotFound()
        {
            // arrange
            const int ticketId = 1;
            _mockTicketService.Setup(x => x.PayTicket(ticketId))
                .ThrowsAsync(new TicketNotFoundException(ticketId));
            
            // act
            var result = await _controller.Post(ticketId, new PaymentRequest {TicketId = ticketId});
            
            // assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task PostReturnsPaymentResponse()
        {
            // arrange
            // act
            var result = await _controller.Post(1, new PaymentRequest {TicketId = 1});
            
            // assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<PaymentResponse>(okResult.Value);
        }
    }
}