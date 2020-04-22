﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Seat
    {
        public int Id { get; set; }
        [Required]
        public string Row { get; set; }
        [Required]
        public int Col { get; set; }
        [Required]
        [ForeignKey("Event")]
        public int EventId { get; set; }
        [Required]
        public bool Availability { get; set; }
        [Required]
        [ForeignKey("SeatType")]
        public int TypeId { get; set; }
        public int? TicketId { get; set; }
        public int? ReservationId { get; set; }

        public virtual Event Event { get; set; }
        public virtual SeatType SeatType { get; set; }
        [ForeignKey("ReservationId")]
        public virtual Reservation Reservation { get; set; }
        [ForeignKey("TicketId")]
        public virtual Ticket Ticket { get; set; }
    }
}
