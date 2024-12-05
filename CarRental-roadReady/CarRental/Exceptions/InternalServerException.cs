using System;

namespace CarRental.Exceptions
{
    public class InternalServerException : Exception
    {
        public InternalServerException(string message) : base(message) { }
    }
}
