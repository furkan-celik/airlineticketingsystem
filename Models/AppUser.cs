using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public enum Genders { Male, Female, NonBinary }

    public class AppUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public DateTime? Birthday { get; set; }
        public Genders Gender { get; set; }
        public string TC { get; set; }

        [ForeignKey("Company")]
        public int? ManagingCompanyId { get; set; }

        public virtual Company ManagingCompany { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<CreditCard> CreditCards { get; set; }
    }
}
