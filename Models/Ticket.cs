using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        [Required]
        [ForeignKey("AppUser")]
        public string OwnerId { get; set; }
        [Required]
        public DateTime ProcessTime { get; set; }
        [Required]
        public int EventId { get; set; }

        public AppUser Owner { get; set; }
        [ForeignKey("EventId")]
        public Event Event { get; set; }
        public ICollection<OfferTicket> Offers { get; set; }
        public ICollection<Seat> Seats { get; set; }
    }
}
