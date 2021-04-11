using System;
using System.ComponentModel.DataAnnotations;

namespace TicketMaster.Api.Model
{
    public class TicketJSONRequest
    {
        [Required]        
        public long ShowId { get; set; }
        [Required]
        [Range(1,15,ErrorMessage ="Quantity should be between 1 and 15")]
        public int Quantity { get; set; }
        [Range(1, 1000, ErrorMessage = "Price should be between 1 and 1000")]
        public Decimal Price { get; set; }
    }
}
