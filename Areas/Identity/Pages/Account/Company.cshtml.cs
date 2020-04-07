using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using WebApplication1.Models;

namespace WebApplication1.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class CompanyModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<CompanyModel> _logger;




      public CompanyModel(
          UserManager<AppUser> userManager,
          SignInManager<AppUser> signInManager,
          ILogger<CompanyModel> logger,
          IEmailSender emailSender)
            {
                    _userManager = userManager;
                    _signInManager = signInManager;
                    _logger = logger;

            }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }


        public class InputModel
        {
          

            [Required]
            [PersonalData]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
            [Display(Name = "Company Name")]
            public string Name { get; set; }

            [Required]
            [PersonalData]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
            [Display(Name = "Company Description")]
            public string Description { get; set; }

            [PersonalData]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
            [Display(Name = "Company Logo")]
            public string LogoLocation { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var comp = new Company { Name = Input.Name, Description= Input.Description };
                //var result = await _userManager.CreateAsync(comp, Input.Name);
                //if (result.Succeeded)
                //{
                //    _logger.LogInformation("User created a new account with password.");

                //    var code = await _userManager.GenerateEmailConfirmationTokenAsync(company);
                //    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                //    var callbackUrl = Url.Page(
                //        "/Account/ConfirmEmail",
                //        pageHandler: null,
                //        values: new { area = "Identity", userId = company.Id, code = code },
                //        protocol: Request.Scheme);



                //    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                //    {
                //        return RedirectToPage("RegisterConfirmation", new { Id = Input.Id });
                //    }
                //    else
                //    {
                //        await _signInManager.SignInAsync(company, isPersistent: false);
                //        return LocalRedirect(returnUrl);
                //    }
                //}
                //foreach (var error in result.Errors)
                //{
                //    ModelState.AddModelError(string.Empty, error.Description);
                //}
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

    }




}
