using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public DateTime Birthday { get; set; }
        public short Gender { get; set; }
        [Required]
        public string TC { get; set; }

        public ICollection<Address> Addresses { get; set; }
        public ICollection<CreditCard> CreditCards { get; set; }
    }
}
