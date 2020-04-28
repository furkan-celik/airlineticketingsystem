using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Airport
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string AirportName { get; set; }
        public int CityId { get; set; }

        [ForeignKey("CityId")]
        public virtual City City { get; set; }
    }
}
