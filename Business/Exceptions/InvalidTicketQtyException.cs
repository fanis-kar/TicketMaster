using System;

namespace TicketMaster.Business.Exceptions
{
    public class InvalidTicketQtyException : Exception
    {
        public InvalidTicketQtyException(int qty) : base(String.Format("{0} tickets is over the allowed limit", qty)) { }
    }
}
