using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using ParkingLot.Api.Controllers;
using ParkingLot.Data.Models;
using ParkingLot.Tickets;

namespace ParkingLot.Api.Tests
{
    public class TicketsControllerTests
    {
        [Fact]
        public async Task GetByIdReturnsOkObjectResult()
        {
            // arrange
            var config = new ParkingLotConfig {MaxParkingSpaces = 5};
            var ticketServiceMock = new Mock<ITicketService>();
            var controller = new TicketsController(config, ticketServiceMock.Object);

            const int ticketId = 2;
            ticketServiceMock.SetupGet(x => x.Queryable).Returns(new[]
            {
                new Ticket {Id = ticketId, Customer = "Test"}
            }.AsQueryable());
            
            // act
            var result = await controller.Get(ticketId);
            
            // assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}