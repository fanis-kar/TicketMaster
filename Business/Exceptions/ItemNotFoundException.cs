using System;

namespace TicketMaster.Business.Exceptions
{
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(long id) : base(String.Format("Item with id={0} not found",id)) { }
    }
}
