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
        [ForeignKey("Flight")]
        public int? FlightId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public float ChildPrice { get; set; }
        public int type { get; set; }

        public virtual Flight Flight { get; set; }
        public virtual ICollection<OfferTicket> Tickets { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
