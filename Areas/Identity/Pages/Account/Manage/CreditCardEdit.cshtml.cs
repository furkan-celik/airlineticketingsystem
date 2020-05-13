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
    public class CreditcardEditModel : PageModel
    {
        private readonly WebApplication1.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private string hashedCreditCard;

        public CreditcardEditModel(WebApplication1.Data.ApplicationDbContext context,
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
        public CreditCard CreditCard { get; set; }

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

        private async Task LoadAsync(int? id)
        {


            CreditCard = await _context.CreditCards.FindAsync(id);

            Input = new InputModel
            {
                CardNumber = CreditCard.CardNumber,
                Month = CreditCard.Month,
                Year = CreditCard.Year
            };
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CreditCard = await _context.CreditCards.FindAsync(id);

            if (CreditCard == null)
            {
                return NotFound();
            }
            await LoadAsync(id);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var cardToUpdate = await _context.CreditCards.FindAsync(id);

            if (cardToUpdate == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var Userid = _signInManager.Context.User.Claims.FirstOrDefault().Value;
            cardToUpdate.OwnerId = Userid;
            cardToUpdate.Owner = user;
            DateTime checkdate = DateTime.Now;

            if (Input.CardNumber.ToString().Length != 16)
            {
                ViewData["Err"] = "Card Number is not valid";
            }
            else if (Input.Month == 0 || Input.Month > 12 || Input.Month < 0)
            {
                ViewData["Err"] = "Expiry Month is not valid";
            }
            else if (Input.Year.ToString().Length != 4)
            {
                ViewData["Err"] = "Expiry Year is not valid";
            }
            else if ((Input.Month < checkdate.Month && Input.Year == checkdate.Year) || (Input.Year < checkdate.Year))
            {
                ViewData["Err"] = "Card has expired. Please enter a valid card.";
            }
            else
            {
                if (cardToUpdate.Month != Input.Month)
                {
                    cardToUpdate.Month = Input.Month;
                }
                if (cardToUpdate.Year != Input.Year)
                {
                    cardToUpdate.Year = Input.Year;
                }

                string hashCardNumber = Input.CardNumber.ToString().Substring(0, 12);

                using (SHA256 sha256Hash = SHA256.Create())
                {
                    // ComputeHash - returns byte array  
                    byte[] hashValue = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(hashCardNumber));

                    StringBuilder result = new StringBuilder();
                    for (int i = 0; i < hashValue.Length; i++)
                    {
                        //result.Append(hashValue[i].ToString());
                        result.Insert(i, hashValue[i].ToString("x2"));
                    }

                    hashedCreditCard = result.ToString();

                }

                hashedCreditCard += Input.CardNumber.ToString().Substring(12, 4);

                if (cardToUpdate.CardNumber != Int64.Parse(Input.CardNumber.ToString().Substring(12, 4)))
                {
                    cardToUpdate.CardNumber = Int64.Parse(Input.CardNumber.ToString().Substring(12, 4));
                }
                if (cardToUpdate.HashedCardNumber != hashedCreditCard)
                {
                    cardToUpdate.HashedCardNumber = hashedCreditCard;
                }

                //CreditCardVM.CardNumber = Int64.Parse(Input.CardNumber.ToString().Substring(12, 4));
                //CreditCardVM.HashedCardNumber = hashedCreditCard;

                var entry = _context.CreditCards.Update(cardToUpdate);
                entry.CurrentValues.SetValues(cardToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToPage("./CreditCardIndex");
            }
            return Page();
        }

        private bool AddressExists(int id)
        {
            return _context.Addresses.Any(e => e.Id == id);
        }

    }
}
