using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TicketMaster.Data.Interfaces;
using TicketMaster.Data.Model;

namespace TicketMaster.Data.Repositories
{
    class EventRepository : Repository<Event>, IEventRepository
    {
        private DbSet<Event> _events;
        public EventRepository(DbContext context):base(context)
        {
            _events = context.Set<Event>();
        }
            public ICollection<Event> GetEventsByDateRange(DateTime StartOfRange, DateTime EndOfRange)
        {
            
        }
    }
}
