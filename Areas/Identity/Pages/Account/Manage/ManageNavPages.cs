using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.Areas.Identity.Pages.Account.Manage
{
    public static class ManageNavPages
    {
        public static string Index => "Index";

        public static string Email => "Email";

        public static string ChangePassword => "ChangePassword";

        public static string Addresses => "Addresses";

        public static string CreditCards => "CreditCards";

        public static string Tickets => "Tickets";

        public static string Reservations => "Reservations";

        public static string CheckIn => "CheckIn";

        public static string ExternalLogins => "ExternalLogins";

        public static string PersonalData => "PersonalData";

        public static string TwoFactorAuthentication => "TwoFactorAuthentication";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string EmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, Email);

        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        public static string AddressesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Addresses);

        public static string CreditCardsNavClass(ViewContext viewContext) => PageNavClass(viewContext, CreditCards);

        public static string TicketsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Tickets);

        public static string ReservationsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Reservations);

        public static string CheckInNavClass(ViewContext viewContext) => PageNavClass(viewContext, CheckIn);

        public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);

        public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);

        public static string TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
