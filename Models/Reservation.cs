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
        [ForeignKey("User")]
        public string UserId { get; set; }
        [Required]
        [ForeignKey("Event")]
        public int EventId { get; set; }

        public AppUser Owner { get; set; }
        public Event Event { get; set; }
    }
}
