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
using System.Diagnostics;
using MimeKit;
using MailKit.Net.Smtp;

using System.Reflection.Metadata.Ecma335;

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

            List<string> list = _context.Airports.Select(x => x.AirportName).ToList();
            List<string> list2 = _context.Cities.Select(x => x.CityName).ToList();
            list2.AddRange(list);

            ViewData["AirportId"] = new SelectList(_context.Airports, "Id", "AirportName");
            ViewData["Err"] = "";
            ViewData["Date"] = @DateTime.Now.ToString("yyyy-MM-dd");

            return View(list2);
        }

        public class SearchResultModel
        {
            public Flight Flight { get; set; }
            public int Price { get; set; }
        }

        public IActionResult SearchResults(string arr, string dest, DateTime date, int numOfAdult, int numOfChild, string ticketClass)
        {
            var flights = from selectList in _context.Flights.Include(x => x.Organizer)
                          select selectList;
            List<SearchResultModel> srm = new List<SearchResultModel>();

            if (string.Equals(arr, dest))
            {
                ViewData["Err"] = "Destination and Arrival can't be the same. Please do another search.";

                foreach (var item in flights)
                {
                    srm.Add(new SearchResultModel() { Flight = item, Price = 0 });
                }
            }
            else
            {
                var d = _context.Airports.Where(x => x.AirportName == dest).ToList();
                var a = _context.Airports.Where(x => x.AirportName == arr).ToList();
                if (d.Count == 0 && a.Count == 0)
                {
                    int c_id_dest = _context.Cities.Where(x => x.CityName == dest).Select(x => x.CityId).FirstOrDefault();
                    List<int> air_id_dest = _context.Airports.Where(x => x.CityId == c_id_dest).Select(x => x.Id).ToList();
                    List<int> r_id_dest = _context.Routes.Where(x => air_id_dest.Contains(x.DepartureId)).Select(x => x.RouteId).ToList();

                    int c_id_arr = _context.Cities.Where(x => x.CityName == arr).Select(x => x.CityId).FirstOrDefault();
                    List<int> air_id_arr = _context.Airports.Where(x => x.CityId == c_id_arr).Select(x => x.Id).ToList();
                    List<int> r_id_arr = _context.Routes.Where(x => air_id_arr.Contains(x.ArrivalId)).Select(x => x.RouteId).ToList();

                    flights = flights.Where(x => r_id_arr.Contains(x.RouteId) && r_id_dest.Contains(x.RouteId));
                }
                else if (d.Count == 0)
                {
                    int c_id = _context.Cities.Where(x => x.CityName == dest).Select(x => x.CityId).FirstOrDefault();
                    List<int> air_id = _context.Airports.Where(x => x.CityId == c_id).Select(x => x.Id).ToList();
                    List<int> r_id = _context.Routes.Where(x => air_id.Contains(x.DepartureId)).Select(x => x.RouteId).ToList();

                    flights = flights.Where(x => x.Route.ArrivalId == a.ElementAt(0).Id && r_id.Contains(x.RouteId));
                }
                else if (a.Count == 0)
                {
                    int c_id = _context.Cities.Where(x => x.CityName == arr).Select(x => x.CityId).FirstOrDefault();
                    List<int> air_id = _context.Airports.Where(x => x.CityId == c_id).Select(x => x.Id).ToList();
                    List<int> r_id = _context.Routes.Where(x => air_id.Contains(x.ArrivalId)).Select(x => x.RouteId).ToList();

                    flights = flights.Where(x => r_id.Contains(x.RouteId) && x.Route.DepartureId == d.ElementAt(0).Id);
                }
                else
                {
                    flights = flights.Where(x => x.Route.ArrivalId == a.ElementAt(0).Id && x.Route.DepartureId == d.ElementAt(0).Id);
                }

                if (date.Ticks > 0)
                {
                    flights = flights.Where(x => x.Date.Date == date.Date);
                }

                if (!string.IsNullOrEmpty(ticketClass))
                {
                    flights = flights.Where(x => x.Offers.Count(y => y.Name == ticketClass) > 0).Include(x => x.Offers);
                }

                if (numOfAdult < 0) numOfAdult = 1;
                if (numOfChild < 0) numOfChild = 0;

                foreach (var item in flights.ToArray())
                {
                    var price = (int)(item.Offers.FirstOrDefault(x => x.Name == ticketClass).ChildPrice * numOfChild + item.Offers.FirstOrDefault(x => x.Name == ticketClass).Price * numOfAdult);
                    srm.Add(new SearchResultModel() { Flight = item, Price = price });
                }
            }

            return PartialView(srm);
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
            if(user != null)
            {
                if (user.ManagingCompanyId == selectedflight.CompanyId)
                {
                    ViewData["samecomp"] = true;
                }
                else
                {
                    ViewData["samecomp"] = false;
                }
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
            flight.Name = flightMan.ElementAt(0).DepartureAirport.AirportName + "-" + flightMan.ElementAt(0).ArrivalAirport.AirportName;
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

        public class OfferInput
        {
            public Offer offer { get; set; }
            public int quantity { get; set; }
            public int childQuantity { get; set; }
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
            public List<OfferInput> offers { get; set; }
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
            inputModel.offers = new List<OfferInput>();
            flight.Offers.Where(x => x.type != 0).ToList().ForEach(x => inputModel.offers.Add(new OfferInput() { offer = x, quantity = 0 }));

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
            var flight = await _context.Flights
         .Include(x => x.Organizer)
         .FirstOrDefaultAsync(m => m.Id == inputModel.flightInfo.Id);

            var seats = _context.Seats
                .Include(x => x.SeatType)
                .Where(x => x.FlightId == flight.Id).ToList();

            var selectedOffers = _context.Offers.Where(x => x.FlightId == flight.Id).ToList();

            List<Seat> selectedSeats = seats.Where(x => x.Availability && !inputModel.seats[x.Col - 1][x.Row.ToCharArray()[0] - 'a'].Availability).ToList();

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
                ViewData["Err"] = "There isn't enough seats for you to buy. Your seat may be taken.";
                return View(flight);
            }
            else if (countOfBaby > countOfSeats)
            {
                ViewData["Err"] = "Infants cannot be more than adults";
                return View(flight);
            }
            else
            {
                Purchase purchase = new Purchase() { IsProcessed = false, OwnerId = _userManager.GetUserId(HttpContext.User), Price = 0, ProcessTime = DateTime.Now };
                purchase.Price += _context.Offers.FirstOrDefault(x => x.Id == type).Price * (countOfChild + countOfSeats);
                purchase.Tickets = new List<Ticket>();

                int counter = 0;
                while (counter < countOfSeats + countOfChild)
                {
                    Ticket tic = new Ticket();
                    tic.ProcessTime = DateTime.Now;
                    tic.EventId = inputModel.flightInfo.Id;
                    tic.OwnerId = _userManager.GetUserId(HttpContext.User);
                    tic.isChild = counter < countOfSeats ? false : true;
                    _context.Add(tic);
                    await _context.SaveChangesAsync();

                    foreach (var item in inputModel.offers)
                    {
                        if (!tic.isChild && item.quantity > 0)
                        {
                            OfferTicket tmp = new OfferTicket() { Offer = selectedOffers.Find(x => x.Id == item.offer.Id), Ticket = tic };
                            _context.OfferTickets.Add(tmp);
                            if (_context.SaveChanges() > 0)
                            {
                                purchase.Price += tmp.Offer.Price;
                                item.quantity--;
                            }
                        }
                        else if (tic.isChild && item.childQuantity > 0)
                        {
                            OfferTicket tmp = new OfferTicket() { Offer = selectedOffers.Find(x => x.Id == item.offer.Id), Ticket = tic };
                            _context.OfferTickets.Add(tmp);
                            if (_context.SaveChanges() > 0)
                            {
                                purchase.Price += tmp.Offer.ChildPrice;
                                item.childQuantity--;
                            }
                        }
                    }

                    Seat seat = selectedSeats.ElementAt(counter);
                    seat.TicketId = (int)tic.Id;
                    seat.Availability = false;
                    seat.TypeId = 1;
                    var tmp2 = _context.Seats.Update(seat);
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        throw;
                    }

                    counter++;
                    purchase.Tickets.Add(tic);
                }

                _context.Purchases.Add(purchase);
                _context.SaveChanges();

                return RedirectToAction("Purchase", new { id = purchase.Id });
            }
        }

        // GET: Flights/LoginModal
        public IActionResult LoginModal()
        {
            return PartialView();
        }
        // GET: Flights/RegisterModal
        public IActionResult RegisterModal()
        {
            return PartialView();
        }

        public IActionResult Successful()
        {
            return View();
        }

        public IActionResult Purchase(int id)
        {
            var purchase = _context.Purchases.FirstOrDefault(x => x.Id == id);

            if(purchase == null)
            {
                return NotFound();
            }
            
            string OwnerId = _userManager.GetUserId(HttpContext.User);
            var usercard = _context.CreditCards.Where(x => x.OwnerId == OwnerId).ToList();
            var cardlist = new SelectList(usercard, "Id", "CardNumber");
            ViewData["CardId"] = cardlist;
            ViewData["Creditcards"] = "Select a card.";


            return View(purchase);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Purchase(int pId, string cardNumber, string cardExpiry, string cardCVC, string couponCode)
        {
            string OwnerId = _userManager.GetUserId(HttpContext.User);
            var usercard = _context.CreditCards.Where(x => x.OwnerId == OwnerId).ToList();
            var cardlist = new SelectList(usercard, "Id", "CardNumber");
            ViewData["CardId"] = cardlist;
            ViewData["Creditcards"] = "Select a card.";


            var purchase = _context.Purchases.FirstOrDefault(x => x.Id == pId);
            purchase.IsProcessed = true;
            _context.Purchases.Update(purchase);
            _context.SaveChanges();


            String mail = _context.Users.Where(a => a.Id == _userManager.GetUserId(HttpContext.User)).Select(a => a.Email).FirstOrDefault().ToString();
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("flightviewerticket@gmail.com"));
            message.To.Add(new MailboxAddress(mail));
            message.Subject = "test";
            message.Body = new TextPart("test")
            {
                Text = "test"
            };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("flightviewerticket@gmail.com", "Cs308proje");

                client.Send(message);
                client.Disconnect(true);
            }

            return RedirectToAction(nameof(Successful));
        }

        public IActionResult Ticket(int id)
        {
            return View(_context.Tickets.FirstOrDefault(x => x.Id == id));
        }

        public IActionResult ListTicket(int id)
        {
            return View();
        }

        private bool FlightExists(int id)
        {
            return _context.Flights.Any(e => e.Id == id);
        }
    }
}
