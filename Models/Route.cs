using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Route
    {
        [Key]
        public int RouteId { get; set; }
        [Required]
        public int DepartureId { get; set; }
        [Required]
        public int ArrivalId { get; set; }
        public TimeSpan ETA { get; set; }

        [ForeignKey("DepartureId")]
        public virtual Airport DepartureAirport { get; set; }
        [ForeignKey("ArrivalId")]
        public virtual Airport ArrivalAirport { get; set; }
        public virtual ICollection<Flight> Flights { get; set; }
    }
}
