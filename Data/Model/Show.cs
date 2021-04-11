using System;
using System.Collections.Generic;

namespace TicketMaster.Data.Model
{
    public class Show : Item
    {
        public long ActId { get; set; }
        public virtual Act Act { get; set; }
        public long VenueId { get; set; }
        public virtual Venue Venue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
