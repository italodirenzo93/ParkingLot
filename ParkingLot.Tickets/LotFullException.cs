using System;

namespace ParkingLot.Tickets
{
    public sealed class LotFullException : Exception
    {
        private readonly int _maxSpaces;

        public LotFullException(int maxSpaces)
        {
            _maxSpaces = maxSpaces;
            this.Data.Add(nameof(maxSpaces), maxSpaces);
        }

        public override string Message => $"Parking Lot is full. {_maxSpaces}/{_maxSpaces} spaces occupied.";
    }
}