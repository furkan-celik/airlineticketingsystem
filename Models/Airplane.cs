using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Airplane
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public int BusinessRowNo { get; set; }
        [Required]
        public int BusinessColumnNo { get; set; }
        [Required]
        public int EconomyRowNo { get; set; }
        [Required]
        public int EconomyColumnNo { get; set; }
        [Required]
        public int SuperCheapRowNo { get; set; }
        [Required]
        public int SuperCheapColumnNo { get; set; }

        public virtual ICollection<Flight> Flights { get; set; }
    }
}
