using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Class;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PurchasesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public PurchasesController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Policy = "ReqAdmin")]
        // GET: Purchases
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("CompAdmin"))
            {
                var applicationDbContext = _context.Purchases.Where(x => x.Tickets.Count > 0)
                    .Include(p => p.Owner).Include(p => p.Tickets);
                //applicationDbContext.Where(Tickets)
                var purchaseList = await applicationDbContext.ToListAsync();
                return View(purchaseList.FindAll(x => x.Tickets.ToList()[0].Flight.CompanyId == _userManager.GetUserAsync(User).Result.ManagingCompanyId));
            }
            else
            {
                var applicationDbContext = _context.Purchases.Include(p => p.Owner).Include(p => p.Tickets);
                return View(await applicationDbContext.ToListAsync());
            }
        }

        [Authorize(Policy = "ReqAdmin")]
        // GET: Purchases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        [Authorize]
        // GET: Purchases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchase == null)
            {
                return NotFound();
            }
            
                return View(purchase);
        }

        [Authorize]
        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticketList = _context.Tickets.Where(x => x.Seats.Count>0)
                .Where(a => a.PurchaseId == id).ToList();
            int sizeoftickets = ticketList.Count();
            for (int i = 0; i < sizeoftickets; i++)
            {
                Ticket t = ticketList.ElementAt(i);
                var seatList = _context.Seats
                    .Where(a => a.TicketId == t.Id).ToList();
                
                Seat seat = seatList.ElementAt(0);
                seat.TicketId = null;
                seat.Availability = true;
                seat.ReservationId = null;
                seat.IsConfirmed = false;
                _context.Update(seat);
                await _context.SaveChangesAsync();

            }
            var purchase = await _context.Purchases.FindAsync(id);

            String msg;
            String userId = _userManager.GetUserId(HttpContext.User);
            MailAdapter mailAdapter = new MailAdapter();

            msg = "Thank you for your ticket purchase. Here are the details < br /> ";
            msg = msg + "Flight: " + purchase.Tickets.ToList()[0].Flight.Name + "<br />" + "Fllght NO : " + purchase.Tickets.ToList()[0].Flight.FlightNo + " < br /> ";
            msg = msg + "Ticket(s) refunded by refund money of: " + "<br />" + purchase.Price * (purchase.Tickets.ToList()[0].Flight.RefundPortion / 100f) + "<br />" + "Refunded price will be in your account in 3 to 5 work days.< br /><br />";
            String to = _context.Users.Where(a => a.Id == userId).Select(a => a.Email).FirstOrDefault().ToString();
            mailAdapter.SendMail(_userManager.GetUserId(HttpContext.User), msg, to);

            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();

            if (User.IsInRole("CompAdmin") && User.IsInRole("WebAdmin"))
                return RedirectToAction(nameof(Index));
            else
                return RedirectToAction("Search", "Flights");
        }

        private bool PurchaseExists(int id)
        {
            return _context.Purchases.Any(e => e.Id == id);
        }
    }
}
