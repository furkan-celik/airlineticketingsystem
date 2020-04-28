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
using System.Web;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApplication1.Controllers
{
    public class FlightsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public FlightsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Events
        public IActionResult Index(int? arr, int? dest, DateTime date)
        {
            //var applicationDbContext = _context.Events.Include(x => x.Organizer);
            //return View(await applicationDbContext.ToListAsync());

            var list = new SelectList(_context.Airports, "Id", "AirportName");
            ViewData["AirportId"] = list;
            ViewData["Err"] = "";
            ViewData["Date"] = @DateTime.Now.ToString("yyyy-MM-dd");

            var flights = from selectList in _context.Flights.Include(x => x.Organizer)
                    select selectList;

            if (arr.HasValue && dest.HasValue)
            {
                ViewData["Date"] = date.ToString("yyyy-MM-dd");
                if (string.Equals(arr, dest))
                {
                    ViewData["Err"] = "Destination and Arrival can't be the same. Please do another search.";

                }
                else {
                    flights = flights.Where(x => x.Route.ArrivalId == arr && x.Route.DepartureId == dest);

                    if(date.Ticks > 0)
                    {
                        flights = flights.Where(x => x.Date.Date == date.Date);
                    }
                }
                
            }
            return View(flights.ToList());
        }

        public IActionResult Search()
        {
            if(User.IsInRole("WebAdmin") || User.IsInRole("CompAdmin"))
            {
                return RedirectToAction("Index");
            }

            ViewData["AirportId"] = new SelectList(_context.Airports, "Id", "AirportName");
            ViewData["Err"] = "";
            ViewData["Date"] = @DateTime.Now.ToString("yyyy-MM-dd");

            return View();
        }

        public IActionResult SearchResults(int arr, int dest, DateTime date)
        {
            var flights = from selectList in _context.Flights.Include(x => x.Organizer)
                          select selectList;

            if (string.Equals(arr, dest))
            {
                ViewData["Err"] = "Destination and Arrival can't be the same. Please do another search.";

            }
            else
            {
                flights = flights.Where(x => x.Route.ArrivalId == arr && x.Route.DepartureId == dest);

                if (date.Ticks > 0)
                {
                    flights = flights.Where(x => x.Date.Date == date.Date);
                }
            }

            return PartialView(flights.Include(x => x.Route).ToList());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            
            var user = await _userManager.GetUserAsync(User);
            var selectedflight = await _context.Flights.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }
            if(user.ManagingCompanyId== selectedflight.CompanyId)
            {
                ViewData["samecomp"] = true;
            }
            else
            {
                ViewData["samecomp"] = false;
            }
            var flight = await _context.Flights
                .Include(x => x.Organizer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        [Authorize(Roles = "WebAdmin,CompAdmin")]
        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Description");
            ViewData["RouteId"] = new SelectList(_context.Routes, "RouteId", "RouteId");
            return View();
        }

        [Authorize(Roles = "WebAdmin,CompAdmin")]
        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CompanyId,Name,RefundTime,ResCancelTime,RefundPortion,Date,RouteId,FlightNo")] Flight flight)
        {
            var flightMan = _context.Routes.Where(a => a.RouteId == flight.RouteId).ToList();
            flight.Name = flightMan.ElementAt(0).DepartureAirport + "-" + flightMan.ElementAt(0).ArrivalAirport;
            if (ModelState.IsValid)
            {
                _context.Add(flight);

                _context.SeatTypes.Add(new SeatType() { Name = "Standard" });
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
                    seat.FlightId = flight.Id;
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Description", flight.CompanyId);
            ViewData["RouteId"] = new SelectList(_context.Routes, "RouteId", "RouteId");
            return View(flight);
        }

        [Authorize(Roles = "WebAdmin,CompAdmin")]
        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Description", flight.CompanyId);
            return View(flight);
        }

        [Authorize(Roles = "WebAdmin,CompAdmin")]
        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyId,Name,RefundTime,ResCancelTime,RefundPortion,Date,FlightNo,RouteId")] Flight flight)
        {
            if (id != flight.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flight);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightExists(flight.Id))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Description", flight.CompanyId);
            return View(flight);
        }

        [Authorize(Roles = "WebAdmin,CompAdmin")]
        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .Include(x => x.Organizer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        [Authorize(Roles = "WebAdmin,CompAdmin")]
        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> Buy(int? id)
        {
            ViewData["Err"] = "";
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .Include(x => x.Organizer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        [Authorize(Roles = "User")]
        //POST: Events/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(int id, int type, int countOfSeats, int countOfChild, int countOfBaby)
        {
            //Reservation res = new Reservation();
            //res.EventId = id;
            //res.OwnerId = _userManager.GetUserId(HttpContext.User);
            //_context.Add(res);
            //await _context.SaveChangesAsync();

            var seatList = _context.Seats.Where(a => a.Availability == true && a.FlightId == id && a.TypeId == type).ToList();
            ViewData["Err"] = "";

            if (seatList == null)
            {
                ViewData["Err"] = "There isn't any seat left in choosen class";
                var flight = await _context.Flights
             .Include(x => x.Organizer)
             .FirstOrDefaultAsync(m => m.Id == id);

                if (flight == null)
                {
                    return NotFound();
                }
                return View(flight);
            }
            else if (seatList.Count() < countOfSeats + countOfChild)
            {
                ViewData["Err"] = "There isn't enough seats for you to buy";
                var flight = await _context.Flights
              .Include(x => x.Organizer)
              .FirstOrDefaultAsync(m => m.Id == id);

                if (flight == null)
                {
                    return NotFound();
                }
                return View(flight);
            }
            else if (countOfBaby > countOfSeats)
            {
                ViewData["Err"] = "Infants cannot be more than adults";
                var flight = await _context.Flights
                .Include(x => x.Organizer)
                .FirstOrDefaultAsync(m => m.Id == id);

                if (flight == null)
                {
                    return NotFound();
                }
                return View(flight);
            }
            else
            {
                int counter = 0;
                while (counter < countOfSeats + countOfChild)
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

                return RedirectToAction(nameof(Successful));

            }

        }

        public IActionResult Successful()
        {
            return View();
        }


        private bool FlightExists(int id)
        {
            return _context.Flights.Any(e => e.Id == id);
        }
    }
}
