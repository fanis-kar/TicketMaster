using System.Collections.Generic;
using System.Threading.Tasks;
using TicketMaster.Data.Model;

namespace TicketMaster.Business.Interfaces
{
    public interface ITicketService : IService<Ticket>
    {
        Task<ICollection<Ticket>> ReserveTickets(long showId, int noOfTickets, decimal price);
        Task<ICollection<Ticket>> GetByShow(long showId);
    }
}
