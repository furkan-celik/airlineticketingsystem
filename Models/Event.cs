using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Event
    {
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
        [Required]
        public string FlightNo { get; set; }

        public Company Organizer { get; set; }
        [ForeignKey("FlightNo")]
        public Flight Flight { get; set; }
    }
}
