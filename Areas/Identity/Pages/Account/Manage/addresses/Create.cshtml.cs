using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Areas.Identity.Pages.Account.Manage.addresses
{
    public class CreateModel : PageModel
    {
        private readonly WebApplication1.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public CreateModel(WebApplication1.Data.ApplicationDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        

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

        public IActionResult OnGet()
        {
            //var address = new Address();
            //return View(address);

            Input = new InputModel();
            return Page();
        }


        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                
                return Page();
            }
            var Userid = _signInManager.Context.User.Claims.FirstOrDefault().Value;
            var emptyAdress = new Address();
            AddressVM.OwnerId = Userid;
            AddressVM.Owner = user;
            AddressVM.Name = Input.Name;
            AddressVM.AddressLine = Input.AddressLine;
            /*if (await TryUpdateModelAsync<Address>(
                emptyAdress,
                "address",   // Prefix for form value.
                a => a.Name, a => a.AddressLine))
            {*/

                var entry = _context.Add(new Address());
                entry.CurrentValues.SetValues(AddressVM);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            //}

                
        }
    }
}
