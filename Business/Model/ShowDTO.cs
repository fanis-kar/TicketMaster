using System;
using System.Collections.Generic;

namespace TicketMaster.Business.Model
{
    public class ShowDTO
    {
        public long Id { get; set; }
        public long ActId { get; set; }
        public long VenueId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //public ICollection<TicketDTO> Tickets { get; set; }
    }
}
