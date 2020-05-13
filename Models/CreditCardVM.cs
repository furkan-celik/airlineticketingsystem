using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class CreditCardVM
    {

        public long CardNumber { get; set; }

        public string HashedCardNumber { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public string OwnerId { get; set; }

        public virtual AppUser Owner { get; set; }
    }
}
