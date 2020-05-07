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
        [Required]
        public bool isChild { get; set; }

        public virtual AppUser Owner { get; set; }
        [ForeignKey("EventId")]
        public virtual Flight Flight { get; set; }
        public virtual ICollection<OfferTicket> Offers { get; set; }
        public virtual ICollection<Seat> Seats { get; set; }
    }
}
