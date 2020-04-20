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
using Microsoft.Extensions.Logging;

namespace WebApplication1.Areas.Identity.Pages.Account.Manage
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


        [BindProperty]
        public InputModel Input { get; set; }

        public Address Address { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public AdressVM AddressVM { get; set; }


        public class InputModel
        {
            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Address")]
            public string AddressLine { get; set; }
        }

        private async Task LoadAsync(int? id)
        {
            

            Address = await _context.Addresses.FindAsync(id);
            
            Input = new InputModel
            {
                Name = Address.Name,
                AddressLine = Address.AddressLine
            };
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
            await LoadAsync(id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
          

            var addressToUpdate = await _context.Addresses.FindAsync(id);

            if (addressToUpdate == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }


           // Address.Name = Input.Name;
           // Address.AddressLine = Input.AddressLine;
            var Userid = _signInManager.Context.User.Claims.FirstOrDefault().Value;
            addressToUpdate.OwnerId = Userid;
            addressToUpdate.Owner = user;
            if(addressToUpdate.Name != Input.Name)
            {
                addressToUpdate.Name = Input.Name;
            }
            if(addressToUpdate.AddressLine != Input.AddressLine)
            {
                addressToUpdate.AddressLine = Input.AddressLine;
            }
            
            
            //AddressVM.OwnerId = Userid;
            //AddressVM.Owner = user;
            //AddressVM.Name = Input.Name;
            //AddressVM.AddressLine = Input.AddressLine;

    
            
            //entry.CurrentValues.SetValues(AddressVM);

            

            var entry = _context.Addresses.Update(addressToUpdate);
            entry.CurrentValues.SetValues(addressToUpdate);
            await _context.SaveChangesAsync();
            return RedirectToPage("./AddressIndex");
            
        }

        private bool AddressExists(int id)
        {
            return _context.Addresses.Any(e => e.Id == id);
        }
    }
}
