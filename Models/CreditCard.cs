using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class CreditCard
    {
        public long CardNumber { get; set; }
        public int CardDate { get; set; }
        [Required]
        [ForeignKey("AppUser")]
        public string UserId { get; set; }

        public AppUser Owner { get; set; }
    }
}
