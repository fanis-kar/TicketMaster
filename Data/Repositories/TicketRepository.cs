using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TicketMaster.Data.Interfaces;
using TicketMaster.Data.Model;

namespace TicketMaster.Data.Repositories
{
    public class TicketRepository: Repository<Ticket>, ITicketRepository
    {
        public TicketRepository(IMyDbContextWrapper ctxWrapper) : base(ctxWrapper) { }

        public void Remove(Ticket t)
        {
            this._entities.Remove(t);
        }
    }
}
