using Castle.Components.DictionaryAdapter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        [Required]
        public DateTime ProcessTime { get; set; }
        [Required]
        [ForeignKey("AppUser")]
        public string OwnerId { get; set; }
        [Required]
        public bool IsProcessed { get; set; }
        [Required]
        public float Price { get; set; }

        public virtual AppUser Owner { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
