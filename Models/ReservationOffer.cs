using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class ReservationOffer
    {
        public int ReservationId { get; set; }
        public virtual Reservation Reservation { get; set; }

        public int OfferId { get; set; }
        public virtual Offer Offer { get; set; }
    }
}
