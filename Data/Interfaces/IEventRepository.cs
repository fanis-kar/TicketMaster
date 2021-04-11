using System;
using System.Collections.Generic;
using System.Text;
using TicketMaster.Data.Model;

namespace TicketMaster.Data.Interfaces
{
    interface IEventRepository:IRepository<Event>
    {
        public ICollection<Event> GetEventsByDateRange(DateTime StartOfRange, DateTime EndOfRange);
    }
}
