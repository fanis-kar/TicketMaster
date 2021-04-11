using System;
using System.Collections.Generic;
using System.Text;
using TicketMaster.Data.Model;

namespace TicketMaster.Data.Interfaces
{
    public interface IShowRepository : IRepository<Show>
    {
        ICollection<Show> GetShowsByDateRange(DateTime from, DateTime to);
        ICollection<Show> GetShowsByVenue(long venueId);
    }
}
