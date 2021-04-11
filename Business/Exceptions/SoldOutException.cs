using System;

namespace TicketMaster.Business.Exceptions
{
    public class SoldOutException : Exception
    {
        public SoldOutException(long id) : base(String.Format("Event with id={0} has been sold out", id)) { }
    }
}
