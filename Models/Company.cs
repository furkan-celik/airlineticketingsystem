using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Logo Location")]
        public string LogoLocation { get; set; }
        
        [Required]
        public string Description { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<AppUser> Managers { get; set; }      
    }

 

}
