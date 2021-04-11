using System;
using System.Collections.Generic;
using System.Text;
using TicketMaster.Data.Model;

namespace TicketMaster.Data.Interfaces
{
    public interface ITicketRepository
    {
        void Remove(Ticket ticket);
    }
}
