using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace WebApplication1.Areas.Identity.Pages.Account.Manage.addresses
{
    public class IndexModel : PageModel
    {
        private readonly WebApplication1.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public IndexModel(WebApplication1.Data.ApplicationDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            
        }

        public IList<Address> Address { get;set; }

        

        public async Task OnGetAsync(AppUser user)
        {
            var Userid = _signInManager.Context.User.Claims.FirstOrDefault().Value;
            var userName = await _userManager.GetUserNameAsync(user);

            Address = await _context.Addresses
                .Where(a => a.OwnerId == Userid)
                .Include(a => a.Owner).ToListAsync();
                


        }
    }
}
