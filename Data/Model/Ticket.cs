using System;

namespace TicketMaster.Data.Model
{
    public class Ticket : Item
    {
        public long ShowId { get; set; }
        //public virtual Show Show{ get; set; }
        public Decimal Price { get; set; }

    }
}
