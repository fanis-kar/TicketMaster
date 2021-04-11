using System;

namespace TicketMaster.Business.Exceptions
{
    public class ShowNotFoundException:Exception
    {
        public ShowNotFoundException(long id) : base(String.Format("Event with id={0} not found", id)) { }
    }
}
