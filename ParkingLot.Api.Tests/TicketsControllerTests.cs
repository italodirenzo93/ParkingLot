using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ParkingLot.Api.Controllers;
using ParkingLot.Api.Requests;
using ParkingLot.Api.Responses;
using ParkingLot.Data.Models;
using ParkingLot.Tickets;
using Xunit;

namespace ParkingLot.Api.Tests
{
    public class TicketsControllerTests
    {
        private readonly ParkingLotConfig _config;
        private readonly Mock<ITicketService> _mockTicketService;
        private readonly TicketsController _controller;

        public TicketsControllerTests()
        {
            _config = new ParkingLotConfig {MaxParkingSpaces = 5};
            _mockTicketService = new Mock<ITicketService>();
            _controller = new TicketsController(_config, _mockTicketService.Object);
        }
        
        [Fact]
        public async Task GetAllReturnsAllTicketsResponse()
        {
            // arrange
            var tickets = new List<Ticket>
            {
                new Ticket {Id = 1, Customer = "Test Customer 1", RateLevelId = 1},
                new Ticket {Id = 2, Customer = "Test Customer 2", RateLevelId = 1},
                new Ticket {Id = 3, Customer = "Test Customer 3", RateLevelId = 1}
            };
            _mockTicketService.Setup(x => x.GetAll()).ReturnsAsync(tickets);
            
            // act
            var result = await _controller.Get();
            
            // assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseObj = Assert.IsType<AllTicketsResponse>(okResult.Value);
            Assert.Equal(tickets, responseObj.Tickets);
        }

        [Fact]
        public async Task GetNonExistingTicketReturnsNotFound()
        {
            // arrange
            const int ticketId = 23;
            _mockTicketService.Setup(x => x.GetById(ticketId)).ReturnsAsync((Ticket) null);
            
            // act
            var result = await _controller.Get(ticketId);
            
            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetTicketByIdReturnsInvoice()
        {
            // arrange
            const int ticketId = 12;
            _mockTicketService.Setup(x => x.GetById(ticketId)).ReturnsAsync(new Ticket
            {
                Id = ticketId,
                Customer = "Test Customer",
                RateLevelId = 1,
                RateLevel = new RateLevel
                {
                    Id = 1,
                    Name = "Test Rate"
                }
            });
            
            // act
            var result = await _controller.Get(ticketId);
            
            // assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var invoice = Assert.IsType<InvoiceResponse>(okResult.Value);
            Assert.Equal(ticketId, invoice.TicketId);
        }

        [Fact]
        public async Task LotFullReturnsTooManyRequests()
        {
            // arrange
            var ticketRequest = new Ticket
            {
                Customer = "Test customer",
                RateLevelId = 1
            };
            _mockTicketService.Setup(x => x.IssueNewTicket(ticketRequest.Customer, ticketRequest.RateLevelId))
                .ThrowsAsync(new LotFullException(3));
            
            // act
            var result = await _controller.Post(ticketRequest);
            
            // assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(429, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task PostReturnsCreatedTicketResponse()
        {
            // arrange
            var ticket = new Ticket
            {
                Id = 12,
                Customer = "Test Customer",
                RateLevelId = 1,
                RateLevel = new RateLevel
                {
                    Id = 1,
                    Name = "Some Name"
                }
            };

            _mockTicketService.Setup(x => x.IssueNewTicket(ticket.Customer, ticket.RateLevelId))
                .ReturnsAsync(ticket);
            
            // act
            var result = await _controller.Post(ticket);
            
            // assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
            var responseObj = Assert.IsType<CreatedTicketResponse>(createdAtResult.Value);
            Assert.Equal(ticket.Id, responseObj.Id);
        }
    }
}