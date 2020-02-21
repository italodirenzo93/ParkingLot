using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using ParkingLot.Web.Models;
using ParkingLot.Web.Extensions;

namespace ParkingLot.Web.Services
{
    public class TicketsService
    {
        private const string ApiRoot = "http://localhost:5002/api";

        private readonly HttpClient _http;

        public TicketsService(HttpClient http)
        {
            _http = http;
        }

        public async Task<Ticket[]> GetTicketsAsync()
        {
            var response =  await _http.GetJsonAsync<TicketsResponse>($"{ApiRoot}/tickets");
            return response.Tickets;
        }

        public async Task<Invoice> GetInvoiceAsync(int ticketId)
        {
            var response = await _http.GetJsonAsync<Invoice>($"{ApiRoot}/tickets/{ticketId}");
            return response;
        }

        public async Task<Ticket> CreateNewTicket(string customer, int rateLevelId)
        {
            var response = await _http.PostJsonAsync<Ticket>($"{ApiRoot}/tickets", new { customer, rateLevelId });
            return response;
        }

        public async Task<PayTicketResponse> PayTicket(int ticketId, string creditCard)
        {
            var response = await _http.PostJsonAsync<PayTicketResponse>($"{ApiRoot}/payments/{ticketId}", new { ticketId, creditCard });
            return response;
        }
    }
}
