using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Areas.Identity.Pages.Account.Manage
{
    public class DetailsModel : PageModel
    {
        private readonly WebApplication1.Data.ApplicationDbContext _context;

        public DetailsModel(WebApplication1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Address Address { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            Address = await _context.Addresses
                .AsNoTracking()
                .Include(a => a.Owner).FirstOrDefaultAsync(m => m.Id == id);

            if (Address == null)
            {
                return NotFound();
            }

            return Page();
        }

    }
}
