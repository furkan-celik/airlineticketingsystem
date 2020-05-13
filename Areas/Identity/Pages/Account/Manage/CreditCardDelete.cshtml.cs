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
    public class CreditcardDeleteModel : PageModel
    {
        private readonly WebApplication1.Data.ApplicationDbContext _context;

        public CreditcardDeleteModel(WebApplication1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CreditCard CreditCard { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            CreditCard = await _context.CreditCards
                .AsNoTracking()
                .Include(a => a.Owner).FirstOrDefaultAsync(m => m.Id == id);

            if (CreditCard == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed. Try again";
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creditcard = await _context.CreditCards.FindAsync(id);

            if (creditcard == null)
            {
                return NotFound();
            }

            try
            {
                _context.CreditCards.Remove(creditcard);
                await _context.SaveChangesAsync();
                return RedirectToPage("./CreditCardIndex");
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("./Delete",
                                     new { id, saveChangesError = true });
            }
        }
    }
}
