using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApplication1.Areas.Identity.Pages.Account.Manage;
using Microsoft.Data.SqlClient;


namespace WebApplication1.Controllers
{
    [Authorize(Roles = "User")]
    public class CheckInController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<AppUser> _userManager;

        public CheckInController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> CheckIn(int? id)
        {
            ViewData["Err"] = "";
            if (id == null)
            {
                return NotFound();
            }
            
            var ticket = await _context.Tickets
                .Include(x => x.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }

            var offerticket = _context.OfferTickets.Where(x => x.TicketId == id).ToList();

            if (offerticket.Count != 0)
            {
                string offer1 = "";
                foreach (var item in offerticket)
                {
                    int offerid = item.OfferId;
                    var offer = _context.Offers.FirstOrDefault(x => x.Id == offerid);
                    offer1 += offer.Description + " ";
                }
                ViewData["Offer"] = offer1;
            }
            else
            {
                ViewData["Offer"] = "No offer selected.";
            }

            DateTime date = ticket.Flight.Date;
            TimeSpan eta = ticket.Flight.Route.ETA;

            var arrivaldate = date + eta;
            ViewData["ArrivalDate"] = arrivaldate;

            return View(ticket);

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn(int id, [Bind("Name")] Ticket ticket1)
        { 
            var ticket = _context.Tickets.Find(id);
            ticket.CheckIn = true;
            ticket.Name = ticket1.Name;

            await _context.SaveChangesAsync();
            //_context.SaveChanges();

            return RedirectToAction(nameof(Successful));
        }

        //Successful
        public IActionResult Successful()
        {
            return View();
        } 
    }
}