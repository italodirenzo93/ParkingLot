using System;

namespace ParkingLot.Tickets
{
    public sealed class TicketNotFoundException : Exception
    {
        private readonly int _ticketId;

        public TicketNotFoundException(int ticketId)
        {
            _ticketId = ticketId;
            this.Data.Add(nameof(ticketId), ticketId);
        }

        public override string Message => $"Ticket #{_ticketId} was not found.";
    }
}