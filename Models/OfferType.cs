using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class OfferType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public virtual ICollection<Offer> Offers { get; set; }
    }
}
