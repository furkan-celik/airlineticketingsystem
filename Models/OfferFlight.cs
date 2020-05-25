using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class OfferFlight
    {
        public int OfferId { get; set; }
        [ForeignKey("OfferId")]
        public virtual Offer Offer { get; set; }

        public int FlightId { get; set; }
        [ForeignKey("FlightId")]
        public virtual Flight Flight { get; set; }
    }
}
