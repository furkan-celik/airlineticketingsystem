using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Promotion
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public float Discount { get; set; }
        [Required]
        [ForeignKey("Company")]
        public int? CompanyId { get; set; }

        public virtual Company Organizer { get; set; }

    }
}
