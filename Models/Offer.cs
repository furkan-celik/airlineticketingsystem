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
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public float ChildPrice { get; set; }
        [ForeignKey("OfferType")]
        public int Type { get; set; }
        public int CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
        public virtual OfferType OfferType { get; set; }
        public virtual ICollection<OfferFlight> Flights { get; set; }
        public virtual ICollection<OfferTicket> Tickets { get; set; }
        public virtual ICollection<ReservationOffer> Reservations { get; set; }
    }
}
