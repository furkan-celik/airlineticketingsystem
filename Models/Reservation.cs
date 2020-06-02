using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        [Required]
        public string OwnerId { get; set; }
        [Required]
        [ForeignKey("Flight")]
        public int FlightId { get; set; }
        [Required]
        public bool isChild { get; set; }
        [Required]
        public DateTime processTime { get; set; }

        public virtual AppUser Owner { get; set; }
        public virtual Flight Flight { get; set; }

        public virtual ICollection<Seat> Seats { get; set; }
        public virtual ICollection<ReservationOffer> Offers { get; set; }
    }
}
