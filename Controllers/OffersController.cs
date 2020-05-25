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
                var applicationDbContext = _context.Offers.Include(o => o.Flights);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var applicationDbContext = _context.Offers
                    .Where(o => o.CompanyId == mancompid.Value)
                    .Include(o => o.Flights);
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
                .Include(o => o.Flights)
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
            ViewData["TypeId"] = new SelectList(_context.OfferTypes, "Id", "Name");
            if (User.IsInRole("WebAdmin"))
            {
                ViewData["Company"] = new SelectList(_context.Companies, "Id", "Name");
            }
            else if (User.IsInRole("CompAdmin"))
            {
                var comp = _userManager.GetUserAsync(User).Result.ManagingCompany.Id;
                ViewData["Company"] = new SelectList(_context.Companies.Where(x => x.Id == comp), "Id", "Name");
            }
            return View();
        }

        // POST: Offers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "WebAdmin,CompAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FlightId,Name,Description,Price,ChildPrice,Type,CompanyId")] Offer offer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(offer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["TypeId"] = new SelectList(_context.OfferTypes, "Id", "Name", offer.Type);

            if (User.IsInRole("WebAdmin"))
            {
                ViewData["Company"] = new SelectList(_context.Companies, "Id", "Name");
            }
            else if (User.IsInRole("CompAdmin"))
            {
                var comp = _userManager.GetUserAsync(User).Result.ManagingCompany.Id;
                ViewData["Company"] = new SelectList(_context.Companies.Where(x => x.Id == comp), "Id", "Name");
            }
            return View(offer);
        }

        public class OfferInput
        {
            public Offer offer { get; set; }
            public bool selected { get; set; }
        }

        public class InputModel
        {
            public Flight flightInfo { get; set; }
            public List<OfferInput> offers { get; set; }
        }

        [BindProperty]
        public InputModel inputModel { get; set; }

        // GET: Bills/CreateForCustomer
        [Authorize(Roles = "WebAdmin,CompAdmin")]
        public ActionResult CreateForEvent(int EventId) //, string Flightno
        {
            inputModel = new InputModel();
            inputModel.flightInfo = _context.Flights.FirstOrDefault(a => a.Id == EventId);
            inputModel.offers = new List<OfferInput>();
            _context.Offers.Where(x => x.CompanyId == inputModel.flightInfo.CompanyId).ToList().ForEach(x => inputModel.offers.Add(new OfferInput() { offer = x, selected = false }));
            // TODO: Verify that customer_id is valid and return HttpNotFound() if not.
            return View(inputModel);
        }

        // POST Bills/CreateForCustomer
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "WebAdmin,CompAdmin")]
        public async Task<ActionResult> CreateForEvent(InputModel InputModel)
        {
            //var eventid = offer.FlightId;
            //var selectedevent = await _context.Flights.FindAsync(EventId);
            var user = await _userManager.GetUserAsync(User);

            // TODO: The usual POST stuff like validating and saving the entity.
            InputModel.offers.Where(x => x.selected).ToList().ForEach(x => _context.OfferFlights.Add(new OfferFlight() { FlightId = InputModel.flightInfo.Id, OfferId = x.offer.Id }));
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

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

            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "FlightNo", offer.Flights);
            ViewData["TypeId"] = new SelectList(_context.OfferTypes, "Id", "Name", offer.Type);

            if (User.IsInRole("WebAdmin"))
            {
                ViewData["Company"] = new SelectList(_context.Companies, "Id", "Name");
            }
            else if (User.IsInRole("CompAdmin"))
            {
                var comp = _userManager.GetUserAsync(User).Result.ManagingCompany.Id;
                ViewData["Company"] = new SelectList(_context.Companies.Where(x => x.Id == comp), "Id", "Name");
            }

            return View(offer);
        }

        // POST: Offers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "WebAdmin,CompAdmin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FlightId,Name,Description,Price,ChildPrice,Type,CompanyId")] Offer offer)
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

            if (User.IsInRole("WebAdmin"))
            {
                ViewData["Company"] = new SelectList(_context.Companies, "Id", "Name");
            }
            else if (User.IsInRole("CompAdmin"))
            {
                var comp = _userManager.GetUserAsync(User).Result.ManagingCompany.Id;
                ViewData["Company"] = new SelectList(_context.Companies.Where(x => x.Id == comp), "Id", "Name");
            }

            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "FlightNo", offer.Flights);
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
                .Include(o => o.Flights)
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
