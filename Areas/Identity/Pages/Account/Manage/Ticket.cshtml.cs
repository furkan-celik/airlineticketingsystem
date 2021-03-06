using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Areas.Identity.Pages.Account.Manage
{
    public class TicketModel : PageModel
    {
        private readonly WebApplication1.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public TicketModel(WebApplication1.Data.ApplicationDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IList<Ticket> tickets { get; set; }
        public IList<Purchase> purchases { get; set; }

        public async Task OnGetAsync(AppUser user)
        {
            var Userid = _signInManager.Context.User.Claims.FirstOrDefault().Value;
            var userName = await _userManager.GetUserNameAsync(user);

            tickets = await _context.Tickets
                .Where(a => a.OwnerId == Userid)
                .Where(x => x.PurchaseId.HasValue)
                .OrderByDescending(x => x.Flight.Date)
                .Include(a => a.Flight)
                .Include(a => a.Purchase)
                .Include(a => a.Seats).ToListAsync();

            purchases = await _context.Purchases.Where(x => x.OwnerId == Userid)
                .OrderByDescending(x => x.ProcessTime)
                .Include(x => x.Tickets).ToListAsync();
        }
       
    }
}
