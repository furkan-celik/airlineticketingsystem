using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace WebApplication1.Controllers
{
    [Authorize("ReqAdmin")]
    public class OffersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public OffersController(ApplicationDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string flightno { get; set; }
        // GET: Offers
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var mancompid = user.ManagingCompanyId;
            if (mancompid == null)
            {
                var applicationDbContext = _context.Offers.Include(o => o.Flight);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var applicationDbContext = _context.Offers
                    .Where(o => o.Flight.CompanyId == mancompid)
                    .Include(o => o.Flight);
                return View(await applicationDbContext.ToListAsync());
            }

        }

        // GET: Offers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers
                .Include(o => o.Flight)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }

        [Authorize(Roles = "WebAdmin,CompAdmin")]
        // GET: Offers/Create
        public IActionResult Create()
        {
            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "FlightNo");
            ViewData["TypeId"] = new SelectList(_context.OfferTypes, "Id", "Name");
            return View();
        }

        // POST: Offers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "WebAdmin,CompAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FlightId,Name,Description,Price,ChildPrice,Type")] Offer offer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(offer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "FlightNo", offer.FlightId);
            ViewData["TypeId"] = new SelectList(_context.OfferTypes, "Id", "Name", offer.Type);
            return View(offer);
        }

        // GET: Bills/CreateForCustomer
        [Authorize(Roles = "WebAdmin,CompAdmin")]
        public ActionResult CreateForEvent(int EventId, string Flightno)
        {
            ViewData["flightno"] = Flightno;
            // TODO: Verify that customer_id is valid and return HttpNotFound() if not.
            return View(new Offer { FlightId = EventId });
        }
        // POST Bills/CreateForCustomer
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "WebAdmin,CompAdmin")]
        public async Task<ActionResult> CreateForEvent([Bind("Id,FlightId,Name,Description,Price,ChildPrice,Type")] Offer offer)
        {
            var eventid = offer.FlightId;
            var user = await _userManager.GetUserAsync(User);
            var selectedevent = await _context.Flights.FindAsync(eventid);
            
            // TODO: The usual POST stuff like validating and saving the entity.
            if (ModelState.IsValid)
            {
                 _context.Offers.Add(offer);
                 await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));
            }

            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "FlightNo", offer.FlightId);
            ViewData["TypeId"] = new SelectList(_context.OfferTypes, "Id", "Name", offer.Type);

            return View(offer);
        }

        // GET: Offers/Edit/5
        [Authorize(Roles = "WebAdmin,CompAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers.FindAsync(id);
            if (offer == null)
            {
                return NotFound();
            }

            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "FlightNo", offer.FlightId);
            ViewData["TypeId"] = new SelectList(_context.OfferTypes, "Id", "Name", offer.Type);

            return View(offer);
        }

        // POST: Offers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "WebAdmin,CompAdmin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FlightId,Name,Description,Price,ChildPrice,Type")] Offer offer)
        {
            if (id != offer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(offer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfferExists(offer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "FlightNo", offer.FlightId);
            ViewData["TypeId"] = new SelectList(_context.OfferTypes, "Id", "Name", offer.Type);
            
            return View(offer);
        }

        // GET: Offers/Delete/5
        [Authorize(Roles = "WebAdmin,CompAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers
                .Include(o => o.Flight)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }

        // POST: Offers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "WebAdmin,CompAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var offer = await _context.Offers.FindAsync(id);
            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfferExists(int id)
        {
            return _context.Offers.Any(e => e.Id == id);
        }
    }
}
