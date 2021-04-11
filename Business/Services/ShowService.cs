using AutoMapper;
using System;
using System.Linq;
using System.Collections.Generic;
using TicketMaster.Business.Interfaces;
using TicketMaster.Data.Interfaces;
using TicketMaster.Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace TicketMaster.Business.Services
{

    public class ShowService : Service<Show>, IShowService
    {

        public ShowService(IMyDbContextWrapper ctxWrapper) : base(ctxWrapper) { }       

        public async Task<ICollection<Show>> GetShowsByDateRange(DateTime from, DateTime to)
        {
            return await this.ContextWrapper.Context.Shows.Where(s => s.StartDate.CompareTo(from) >= 0
                      && s.EndDate.CompareTo(to) <= 0)
             .OrderBy(s => s.StartDate).ToListAsync();
        }

        public async Task<ICollection<Show>> GetShowsByVenue(long venueId)
        {                            
                return await this.ContextWrapper.Context.Shows.Where(s => s.Venue.Id == venueId).OrderBy(s => s.StartDate)
                                .ToListAsync();
        }        
    }
}
