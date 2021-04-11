using System;
using System.Collections.Generic;

namespace TicketMaster.Data.Model
{
    public class Venue : Item
    {
        public String Name { get; set; }
        public int Capacity { get; set; }
        //public virtual ICollection<Show> Shows { get; set; }
    }
}
