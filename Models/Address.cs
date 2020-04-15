using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Address
    {
        public int Id { get; set; }
        [Required]
        [ForeignKey("AppUser")]
        public string OwnerId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string AddressLine { get; set; }

        public AppUser Owner { get; set; }
    }
}
