using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Flight
    {
        [Key]
        public string FlightNo { get; set; }
        [Required]
        public string Destination { get; set; }
        [Required]
        public string Arrival { get; set; }
        public DateTime ETA { get; set; }

        public ICollection<Event> Events { get; set; }
        public ICollection<Offer> Offers { get; set; }
    }
}
