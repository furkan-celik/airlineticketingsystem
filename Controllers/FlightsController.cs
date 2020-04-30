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
using System.Diagnostics.CodeAnalysis;

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

        [Authorize("ReqAdmin")]
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
                else
                {
                    flights = flights.Where(x => x.Route.ArrivalId == arr && x.Route.DepartureId == dest);

                    if (date.Ticks > 0)
                    {
                        flights = flights.Where(x => x.Date.Date == date.Date);
                    }
                }

            }
            return View(flights.ToList());
        }

        public IActionResult Search()
        {
            if (User.IsInRole("WebAdmin") || User.IsInRole("CompAdmin"))
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
            if (user.ManagingCompanyId == selectedflight.CompanyId)
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

        [Authorize("ReqAdmin")]
        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Description");
            ViewData["RouteId"] = new SelectList(_context.Routes, "RouteId", "RouteId");
            return View();
        }

        [Authorize("ReqAdmin")]
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

        [Authorize("ReqAdmin")]
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

        [Authorize("ReqAdmin")]
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

        [Authorize("ReqAdmin")]
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

        public class SeatInput
        {
            public int Id { get; set; }
            public int Col { get; set; }
            public string Row { get; set; }
            public bool Availability { get; set; }
        }

        public class SeatInputComparer : IEqualityComparer<SeatInput>
        {
            public bool Equals([AllowNull] SeatInput x, [AllowNull] SeatInput y)
            {
                return (x.Id == y.Id && x.Availability == y.Availability);
            }

            public int GetHashCode([DisallowNull] SeatInput obj)
            {
                return obj.GetHashCode();
            }
        }

        public class InputModel
        {
            public Flight flightInfo { get; set; }
            public List<List<SeatInput>> seats { get; set; }
        }

        [BindProperty]
        public InputModel inputModel { get; set; }

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
                .Include(x => x.Seats)
                .Include(x => x.Offers)
                .FirstOrDefaultAsync(m => m.Id == id);

            ViewData["Offers"] = new SelectList(flight.Offers, "Id", "Name");

            var colGroup = flight.Seats.GroupBy(x => x.Col).ToList();
            inputModel = new InputModel();
            inputModel.flightInfo = flight;
            inputModel.seats = new List<List<SeatInput>>();

            for (int i = 0; i < colGroup.Count; i++)
            {
                List<SeatInput> row = new List<SeatInput>();
                colGroup[i].ToList().ForEach(x => row.Add(new SeatInput { Id = x.Id, Col = x.Col, Row = x.Row, Availability = x.Availability }));
                inputModel.seats.Add(row);
            }

            if (flight == null)
            {
                return NotFound();
            }

            return PartialView(inputModel);
        }

        //POST: Events/
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(int id, int type, int countOfSeats, int countOfChild, int countOfBaby, InputModel inputModel)
        {
            //Reservation res = new Reservation();
            //res.EventId = id;
            //res.OwnerId = _userManager.GetUserId(HttpContext.User);
            //_context.Add(res);
            //await _context.SaveChangesAsync();

            var flight = await _context.Flights
         .Include(x => x.Organizer)
         .FirstOrDefaultAsync(m => m.Id == inputModel.flightInfo.Id);

            List<Seat> selectedSeats = new List<Seat>();
            for (int i = 0; i < inputModel.seats.Count; i++)
            {
                var colGroup = flight.Seats.Where(x => x.Col == inputModel.seats[i][0].Col).ToList();
                List<SeatInput> row = new List<SeatInput>();
                colGroup.ForEach(x => row.Add(new SeatInput { Id = x.Id, Col = x.Col, Row = x.Row, Availability = x.Availability }));

                inputModel.seats[i].Where(x => !x.Availability).Except(row, new SeatInputComparer()).ToList().ForEach(x => selectedSeats.Add(_context.Seats.FirstOrDefault(y => y.Id == x.Id)));
            }

            ViewData["Err"] = "";

            ViewData["Err"] = "There isn't any seat left in choosen class";

            if (flight == null)
            {
                return NotFound();
            }

            if (selectedSeats == null)
            {
                return View(flight);
            }
            else if (selectedSeats.Count() < countOfSeats + countOfChild)
            {
                ViewData["Err"] = "There isn't enough seats for you to buy";
                return View(flight);
            }
            else if (countOfBaby > countOfSeats)
            {
                ViewData["Err"] = "Infants cannot be more than adults";
                return View(flight);
            }
            else
            {
                int counter = 0;
                while (counter < countOfSeats + countOfChild)
                {
                    Ticket tic = new Ticket();
                    tic.ProcessTime = DateTime.Now;
                    tic.EventId = inputModel.flightInfo.Id;
                    tic.OwnerId = _userManager.GetUserId(HttpContext.User);
                    _context.Add(tic);
                    await _context.SaveChangesAsync();

                    Seat seat = selectedSeats.ElementAt(counter);
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
