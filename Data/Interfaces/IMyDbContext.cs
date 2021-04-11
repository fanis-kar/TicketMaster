using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TicketMaster.Data.Model;

namespace TicketMaster.Data.Interfaces
{
    public interface IMyDbContext
    {
        public DbSet<Act> Acts { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Venue> Venues { get; set; }
    }
}
