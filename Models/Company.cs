using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string LogoLocation { get; set; }
        [Required]
        public string Description { get; set; }
        public string Type { get; set; }

        public ICollection<Event> Events { get; set; }
    }
}
