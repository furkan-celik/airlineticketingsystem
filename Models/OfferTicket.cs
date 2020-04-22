using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class OfferTicket
    {
        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }

        public int OfferId { get; set; }
        public virtual Offer Offer { get; set; }
    }
}
