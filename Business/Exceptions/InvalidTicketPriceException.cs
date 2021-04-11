using System;

namespace TicketMaster.Business.Exceptions
{
    public class InvalidTicketPriceException : Exception
    {
        public InvalidTicketPriceException(decimal price) : base(String.Format("Tickets with price {0} do not exist", price)) { }
    }
}
