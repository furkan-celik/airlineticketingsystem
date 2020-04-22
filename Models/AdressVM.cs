using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class AdressVM
    {
        public string OwnerId { get; set; }
        
        public string Name { get; set; }
        
        public string AddressLine { get; set; }

        public virtual AppUser Owner { get; set; }
    }
}
