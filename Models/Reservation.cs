﻿using System;
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
        [ForeignKey("Event")]
        public int EventId { get; set; }

        public virtual AppUser Owner { get; set; }
        public virtual Event Event { get; set; }

        public virtual ICollection<Seat> Seats { get; set; }
    }
}
