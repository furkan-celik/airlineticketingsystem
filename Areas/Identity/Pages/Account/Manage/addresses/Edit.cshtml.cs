using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace WebApplication1.Areas.Identity.Pages.Account.Manage.addresses
{
    public class EditModel : PageModel
    {
        private readonly WebApplication1.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public EditModel(WebApplication1.Data.ApplicationDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public Address Address { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public AdressVM AddressVM { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Name of Address")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Address")]
            public string AddressLine { get; set; }
        }



        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Address = await _context.Addresses.FindAsync(id);

            if (Address == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var addressToUpdate = await _context.Addresses.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);
            if (addressToUpdate == null)
            {
                return NotFound();
            }
            
            var Userid = _signInManager.Context.User.Claims.FirstOrDefault().Value;
            addressToUpdate.OwnerId = Userid;
            addressToUpdate.Owner = user;
            addressToUpdate.Name = Input.Name;
            addressToUpdate.AddressLine = Input.AddressLine;
            //AddressVM.OwnerId = Userid;
            //AddressVM.Owner = user;
            //AddressVM.Name = Input.Name;
            //AddressVM.AddressLine = Input.AddressLine;

            //var entry = _context.(new Address());
            //entry.CurrentValues.SetValues(AddressVM);

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");


            

            return Page();
        }

        private bool AddressExists(int id)
        {
            return _context.Addresses.Any(e => e.Id == id);
        }
    }
}
