using System;

namespace TicketMaster.Api.Model
{
    public class TicketJSONReply
    {
        public long Id { get; set; }
        public long ShowId { get; set; }
        public Decimal Price { get; set; }
    }
}
