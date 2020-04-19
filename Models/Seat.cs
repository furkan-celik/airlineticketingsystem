using System;
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
        public int TicketId { get; set; }
        public int ReservationId { get; set; }

        public Event Event { get; set; }
        public SeatType SeatType { get; set; }
        [ForeignKey("ReservationId")]
        public Reservation Reservation { get; set; }
        [ForeignKey("TicketId")]
        public Ticket Ticket { get; set; }
    }
}
