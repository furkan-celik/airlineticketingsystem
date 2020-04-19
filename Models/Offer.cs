using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Offer
    {
        public int Id { get; set; }
        [ForeignKey("Event")]
        public int EventId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public float Price { get; set; }

        public Event Event { get; set; }
        public ICollection<OfferTicket> Tickets { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
