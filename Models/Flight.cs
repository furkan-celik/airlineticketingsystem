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
        public Flight()
        {
        }

        public Flight(Flight _flight)
        {
            CompanyId = _flight.CompanyId;
            Name = _flight.Name;
            RefundTime = _flight.RefundTime;
            ResCancelTime = _flight.ResCancelTime;
            RefundPortion = _flight.RefundPortion;
            Date = _flight.Date;
            FlightNo = _flight.FlightNo;
            RouteId = _flight.RouteId;
            AirplaneId = _flight.AirplaneId;
        }

        public int Id { get; set; }
        [Required]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        [Required]
        public string Name { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan RefundTime { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan ResCancelTime { get; set; }
        public float RefundPortion { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string FlightNo { get; set; }
        [Required]
        public int RouteId { get; set; }
        [Required]
        [ForeignKey("Airplane")]
        public string AirplaneId { get; set; }

        [ForeignKey("RouteId")]
        public virtual Route Route { get; set; }
        public virtual Company Organizer { get; set; }
        public virtual Airplane Airplane { get; set; }
        public virtual ICollection<Seat> Seats { get; set; }
        public virtual ICollection<OfferFlight> Offers { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }


    }
}
