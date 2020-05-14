using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Areas.Identity.Pages.Account.Manage
{
    public class CreditcardCreateModel : PageModel
    {
        private readonly WebApplication1.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private string hashedCreditCard;

        public CreditcardCreateModel(WebApplication1.Data.ApplicationDbContext context,
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
        public CreditCardVM CreditCardVM { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Card Number")]
            public long CardNumber { get; set; }

            [Required]
            [Display(Name = "Month")]
            public int Month { get; set; }

            [Required]
            [Display(Name = "Year")]
            public int Year { get; set; }
        }

        public IActionResult OnGet()
        {
            Input = new InputModel();
            return Page();
        }

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
            var emptycard = new CreditCard();

            CreditCardVM.OwnerId = Userid;
            CreditCardVM.Owner = user;
            DateTime checkdate = DateTime.Now;


            if (Input.CardNumber.ToString().Length != 16)
            {
                ViewData["Err"] = "Card Number is not valid";
            }
            else if(Input.Month == 0 || Input.Month > 12 || Input.Month < 0)
            {
                ViewData["Err"] = "Expiry Month is not valid";
            }
            else if(Input.Year.ToString().Length != 4)
            {
                ViewData["Err"] = "Expiry Year is not valid";
            }
            else if((Input.Month < checkdate.Month && Input.Year == checkdate.Year) || (Input.Year < checkdate.Year))
            {
                ViewData["Err"] = "Card has expired. Please enter a valid card.";
            }
            else
            {
                string hashCardNumber = Input.CardNumber.ToString();
                CreditCardVM.Month = Input.Month;
                CreditCardVM.Year = Input.Year;

                using (SHA256 sha256Hash = SHA256.Create())
                {
                    // ComputeHash - returns byte array  
                    byte[] hashValue = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(hashCardNumber));

                    StringBuilder result = new StringBuilder();
                    for (int i = 0; i < hashValue.Length; i++)
                    {
                        //result.Append(hashValue[i].ToString());
                        result.Insert(i,hashValue[i].ToString("x2")); 
                    }

                    hashedCreditCard = result.ToString();

                }

                //hashedCreditCard += Input.CardNumber.ToString().Substring(12,4);
                CreditCardVM.CardNumber = Int64.Parse(Input.CardNumber.ToString().Substring(12, 4));
                CreditCardVM.HashedCardNumber = hashedCreditCard;


                var entry = _context.Add(new CreditCard());
                entry.CurrentValues.SetValues(CreditCardVM);
                await _context.SaveChangesAsync();
                return RedirectToPage("./CreditCardIndex");
            }

            return Page();
            

        }
    }
}
