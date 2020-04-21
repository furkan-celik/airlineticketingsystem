using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Controllers
{
    public class EventsController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public EventsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }




        // GET: Events
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Events.Include(x => x.Organizer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(x => x.Organizer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        [Authorize(Roles = "WebAdmin,CompAdmin")]
        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Description");
            ViewData["FlightNo"] = new SelectList(_context.Flights, "FlightNo", "FlightNo");
            return View();
        }

        [Authorize(Roles = "WebAdmin,CompAdmin")]
        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CompanyId,Name,RefundTime,ResCancelTime,RefundPortion,Date,FlightNo")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();

                //Create seats for event
                string seatLet = "abcdef";
                int i = 0;
                int c = 1;
                int r = 0;
                int t = 1;
                while (i < 30)
                {
                    if (c > 2) { t = 2; }
                    Seat seat = new Seat();
                    seat.Id = 0;
                    seat.Row = seatLet.ElementAt(r).ToString();
                    seat.Col = c;
                    seat.EventId = @event.Id;
                    seat.Availability = true;
                    seat.TypeId = t;
                    seat.ReservationId = null;
                    seat.TicketId = null;
                    _context.Add(seat);
                    await _context.SaveChangesAsync();
                    r++;
                    i++;
                    if (r > 5) { r = 0; c++; }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Description", @event.CompanyId);
            ViewData["FlightNo"] = new SelectList(_context.Flights, "FlightNo", "FlightNo");
            return View(@event);
        }

        [Authorize(Roles = "WebAdmin,CompAdmin")]
        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Description", @event.CompanyId);
            return View(@event);
        }

        [Authorize(Roles = "WebAdmin,CompAdmin")]
        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyId,Name,RefundTime,ResCancelTime,RefundPortion,Date,FlightNo")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Description", @event.CompanyId);
            return View(@event);
        }

        [Authorize(Roles = "WebAdmin,CompAdmin")]
        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(x => x.Organizer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        [Authorize(Roles = "WebAdmin,CompAdmin")]
        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Buy(int? id)
        {
            ViewData["Err"] = "";
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(x => x.Organizer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (@event == null)
            {
                return NotFound();
            }



            return View(@event);
        }


        //POST: Events/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(int id, int type, int countOfSeats)
        {
            //Reservation res = new Reservation();
            //res.EventId = id;
            //res.OwnerId = _userManager.GetUserId(HttpContext.User);
            //_context.Add(res);
            //await _context.SaveChangesAsync();

            var seatList = _context.Seats.Where(a => a.Availability == true && a.EventId == id && a.TypeId == type).ToList();
            ViewData["Err"] = "";

            if (seatList == null)
            {
                ViewData["Err"] = "There isn't any seat left in choosen class";
                var @event = await _context.Events
             .Include(x => x.Organizer)
             .FirstOrDefaultAsync(m => m.Id == id);

                if (@event == null)
                {
                    return NotFound();
                }
                return View(@event);
            }
            else if (seatList.Count() < countOfSeats)
            {
                ViewData["Err"] = "There isn't enough seats for you to buy";
                var @event = await _context.Events
              .Include(x => x.Organizer)
              .FirstOrDefaultAsync(m => m.Id == id);

                if (@event == null)
                {
                    return NotFound();
                }
                return View(@event);
            }
            else
            {
                int counter = 0;
                while (counter < countOfSeats)
                {
                    Ticket tic = new Ticket();
                    tic.ProcessTime = DateTime.Now;
                    tic.EventId = id;
                    tic.OwnerId = _userManager.GetUserId(HttpContext.User);
                    _context.Add(tic);
                    await _context.SaveChangesAsync();

                    Seat seat = seatList.ElementAt(counter);
                    seat.TicketId = (int)tic.Id;
                    seat.Availability = false;
                    _context.Update(seat);
                    await _context.SaveChangesAsync();

                    counter++;

                }


                return RedirectToAction(nameof(Index));

            }

        }


        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
