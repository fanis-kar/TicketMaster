using System;
using System.Collections.Generic;
using System.Linq;
using TicketMaster.Data.Interfaces;
using TicketMaster.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace TicketMaster.Data.Repositories
{
    public class ShowRepository : Repository<Show>, IShowRepository
    {
        public ShowRepository(IMyDbContextWrapper ctxWrapper) : base(ctxWrapper) { }
        public ICollection<Show> GetShowsByDateRange(DateTime from, DateTime to)
        {
            return _entities.Where(s => s.StartDate.CompareTo(from) >= 0
                              && s.EndDate.CompareTo(to) <= 0)
                     .OrderBy(s => s.StartDate).ToList();
        }

        public ICollection<Show> GetShowsByVenue(long venueId)
        {
            return _entities.Where(s => s.Venue.Id == venueId).OrderBy(s => s.StartDate)
                            .ToList();
        }     
    }
}
