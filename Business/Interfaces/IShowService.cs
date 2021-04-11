using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketMaster.Data.Interfaces;
using TicketMaster.Data.Model;

namespace TicketMaster.Business.Interfaces
{
    public interface IShowService : IService<Show>
    {        
        Task<ICollection<Show>> GetShowsByDateRange(DateTime from, DateTime to);
        Task<ICollection<Show>> GetShowsByVenue(long venueId);
    }
}
