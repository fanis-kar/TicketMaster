using System;

namespace TicketMaster.Api.Model
{
    public class ShowJSON
    {
        public long Id { get; set; }
        public long ActId { get; set; }
        public long VenueId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
