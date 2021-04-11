using System;

namespace TicketMaster.Business.Model
{
    public class TicketDTO
    {
        public long Id { get; set; }
        public long ShowId { get; set; }
        public int Quantity { get; set; }
        public Decimal Price { get; set; }
    }
}
