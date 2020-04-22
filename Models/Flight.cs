using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Flight
    {
        [Key]
        public string FlightNo { get; set; }
        [Required]
        public string Departure { get; set; }
        [Required]
        public string Arrival { get; set; }
        public DateTime ETA { get; set; }

        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Offer> Offers { get; set; }
    }
}
