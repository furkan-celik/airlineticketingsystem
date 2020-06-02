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
using SQLitePCL;
using WebApplication1.Class;

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
            if (flights != null)
                return View(flights.ToList());
            else
                return View(null);
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

            ViewData["classes"] = new SelectList(_context.OfferTypes.Where(x => x.Id < 4), "Id", "Name");
            ViewData["AirportId"] = new SelectList(_context.Airports, "Id", "AirportName");
            ViewData["Err"] = "";
            ViewData["Date"] = @DateTime.Now.ToString("yyyy-MM-dd");

            return View(list2);
        }

        public IActionResult SearchAsPage(string arr, string dest, DateTime date, int numOfAdult, int numOfChild, int ticketClass, int pId)
        {
            return View(GetListOfFlights(arr, dest, date, numOfAdult, numOfChild, ticketClass));
        }

        public class SearchResultModel
        {
            public Flight Flight { get; set; }
            public int Price { get; set; }
            public DateTime ReturnDate { get; set; }
            public int? pId { get; set; }
        }

        public IActionResult SearchResults(string arr, string dest, DateTime date, int numOfAdult, int numOfChild, int ticketClass, DateTime returnDate)
        {
            return PartialView(GetListOfFlights(arr, dest, date, numOfAdult, numOfChild, ticketClass, returnDate));
        }

        public List<SearchResultModel> GetListOfFlights(string arr, string dest, DateTime date, int numOfAdult, int numOfChild, int ticketClass, DateTime returnDate = default, int? pId = null)
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

                if (ticketClass > 0 && ticketClass < 4)
                {
                    flights = flights.Where(x => x.Offers.Count(y => y.Offer.Type == ticketClass) > 0).Include(x => x.Offers);
                }

                if (numOfAdult < 0) numOfAdult = 1;
                if (numOfChild < 0) numOfChild = 0;

                foreach (var item in flights.ToArray())
                {
                    var price = (int)(item.Offers.FirstOrDefault(x => x.Offer.Type == ticketClass).Offer.ChildPrice * numOfChild + item.Offers.FirstOrDefault(x => x.Offer.Type == ticketClass).Offer.Price * numOfAdult);
                    srm.Add(new SearchResultModel() { Flight = item, Price = price, ReturnDate = returnDate, pId = pId });
                }
            }

            return srm;
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
            if (user != null)
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
                .Include(x => x.Seats)
                .Include(x => x.Reservations)
                .Include(x => x.Tickets)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        public class CreateInputModel
        {
            public Flight Flight { get; set; }
            public List<Airplane> Airplanes { get; set; }
            public List<OfferInput> Offers { get; set; }
            public int RepeatTime { get; set; }
            public int RepeatCount { get; set; }
        }

        [BindProperty]
        public CreateInputModel createInputModel { get; set; }

        [Authorize("ReqCompAdmin")]
        // GET: Events/Create
        public IActionResult Create()
        {
            createInputModel = new CreateInputModel();
            createInputModel.Flight = new Flight();
            createInputModel.Offers = new List<OfferInput>();
            createInputModel.Airplanes = _context.Airplanes.ToList();
            //_context.Offers.Where(x => x.Flight.CompanyId == user.ManagingCompanyId).ToList().ForEach(x => createInputModel.Offers.Add(new OfferInput() { offer = x, selected = false }));
            _context.Offers.Where(x => x.CompanyId == _userManager.GetUserAsync(User).Result.ManagingCompanyId).ToList().ForEach(x => createInputModel.Offers.Add(new OfferInput() { offer = x, selected = false }));
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Description");
            ViewData["RouteId"] = new SelectList(_context.Routes, "RouteId", "RouteId");

            List<SelectListItem> selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem() { Value = "null", Text = "Select an Airplane" });
            foreach (var item in _context.Airplanes.ToList())
            {
                selectListItems.Add(new SelectListItem(item.Id + "Business Row/Col" + item.BusinessRowNo + "/" + item.BusinessColumnNo + ". Economy Row/Col: " + item.EconomyRowNo + "/" + item.EconomyColumnNo, item.Id));
            }
            ViewData["AirplaneId"] = new SelectList(selectListItems, "Value", "Text");

            return View(createInputModel);
        }

        [Authorize("ReqCompAdmin")]
        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInputModel CreateInput)
        {
            if (User.IsInRole("CompAdmin"))
            {
                var user = _userManager.GetUserAsync(User).Result;
                CreateInput.Flight.CompanyId = user.ManagingCompanyId.Value;
            }

            for (int x = 0; x <= CreateInput.RepeatCount; x++)
            {
                var flight = new Flight(CreateInput.Flight);
                var flightMan = _context.Routes.Where(a => a.RouteId == flight.RouteId).ToList();
                flight.Name = flightMan.ElementAt(0).DepartureAirport.AirportName + "-" + flightMan.ElementAt(0).ArrivalAirport.AirportName;
                flight.Date = CreateInput.Flight.Date.AddDays(x * CreateInput.RepeatTime);
                flight.Airplane = _context.Airplanes.Find(flight.AirplaneId);
                _context.Flights.Add(flight);
                await _context.SaveChangesAsync();

                CreateInput.Offers.Where(x => x.selected).ToList().ForEach(x => _context.OfferFlights.Add(new OfferFlight() { Flight = flight, OfferId = x.offer.Id }));
                await _context.SaveChangesAsync();

                for (int i = 0; i < flight.Airplane.BusinessRowNo; i++)
                {
                    for (int j = 1; j <= flight.Airplane.BusinessColumnNo; j++)
                    {
                        AddSeat(i, j, flight.Id, _context.OfferTypes.FirstOrDefault(x => x.Name == "Business").Id);
                    }
                }

                for (int i = 0; i < flight.Airplane.EconomyRowNo; i++)
                {
                    for (int j = 1; j <= flight.Airplane.EconomyColumnNo; j++)
                    {
                        AddSeat(i, j + flight.Airplane.BusinessColumnNo, flight.Id, _context.OfferTypes.FirstOrDefault(x => x.Name == "Economy").Id);
                    }
                }

                for (int i = 0; i < flight.Airplane.SuperCheapRowNo; i++)
                {
                    for (int j = 1; j <= flight.Airplane.SuperCheapColumnNo; j++)
                    {
                        AddSeat(i, j + flight.Airplane.BusinessColumnNo + flight.Airplane.EconomyColumnNo, flight.Id, _context.OfferTypes.FirstOrDefault(x => x.Name == "Super Cheap").Id);
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public void AddSeat(int row, int col, int flightId, int type)
        {
            Seat seat = new Seat();
            seat.Id = 0;
            seat.Row = ((char)(row + (int)'a')).ToString();
            seat.Col = col;
            seat.FlightId = flightId;
            seat.Availability = true;
            seat.TypeId = type;
            seat.ReservationId = null;
            seat.TicketId = null;
            _context.Add(seat);
            _context.SaveChanges();
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
            public int Type { get; set; }
        }

        public class OfferInput
        {
            public Offer offer { get; set; }
            public int quantity { get; set; }
            public int childQuantity { get; set; }
            public bool selected { get; set; }
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
            public int numOfAdult { get; set; }
            public int numOfChild { get; set; }
            public int ticketClass { get; set; }
            public int basePrice { get; set; }
            public DateTime returnDate { get; set; }
            public int PId { get; set; }
        }

        [BindProperty]
        public InputModel inputModel { get; set; }

        [HttpGet]
        public async Task<IActionResult> Buy(int? id, int numOfAdult, int numOfChild, int ticketClass, DateTime? returnDate, int? pId)
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
            inputModel.numOfAdult = numOfAdult;
            inputModel.numOfChild = numOfChild;
            inputModel.ticketClass = ticketClass;
            inputModel.basePrice = (int)(flight.Offers.FirstOrDefault(x => x.Offer.Type == ticketClass).Offer.ChildPrice * numOfChild + flight.Offers.FirstOrDefault(x => x.Offer.Type == ticketClass).Offer.Price * numOfAdult);
            flight.Offers.Where(x => x.Offer.Type == 4).ToList().ForEach(x => inputModel.offers.Add(new OfferInput() { offer = x.Offer, quantity = 0 }));

            for (int i = 0; i < colGroup.Count; i++)
            {
                List<SeatInput> row = new List<SeatInput>();
                colGroup[i].ToList().ForEach(x => row.Add(new SeatInput { Id = x.Id, Col = x.Col, Row = x.Row, Availability = x.Availability, Type = x.TypeId }));
                inputModel.seats.Add(row);
            }

            if (flight == null)
            {
                return NotFound();
            }

            if (returnDate.HasValue)
            {
                inputModel.returnDate = returnDate.Value;
            }
            else
            {
                inputModel.returnDate = default;
                if (pId.HasValue)
                {
                    inputModel.PId = pId.Value;
                }
            }

            return View(inputModel);
        }

        //POST: Events/
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(int id, int countOfBaby, InputModel inputModel)
        {
            var flight = await _context.Flights
         .Include(x => x.Organizer)
         .FirstOrDefaultAsync(m => m.Id == inputModel.flightInfo.Id);

            var seats = _context.Seats
                .Include(x => x.OfferType)
                .Where(x => x.FlightId == flight.Id).ToList();

            var selectedOffers = flight.Offers.Select(x => x.Offer).ToList();

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
            else if (selectedSeats.Count() < inputModel.numOfAdult + inputModel.numOfChild)
            {
                ViewData["Err"] = "There isn't enough seats for you to buy. Your seat may be taken.";
                return View(flight);
            }/*
            else if (countOfBaby > inputModel.numOfAdult)
            {
                ViewData["Err"] = "Infants cannot be more than adults";
                return View(flight);
            }*/
            else
            {
                Purchase purchase;
                if (inputModel.PId == default)
                {
                    purchase = new Purchase() { IsProcessed = false, OwnerId = _userManager.GetUserId(HttpContext.User), Price = 0, ProcessTime = DateTime.Now };
                    purchase.Tickets = new List<Ticket>();
                    purchase.Price = 0;
                }
                else
                {
                    purchase = _context.Purchases.Find(inputModel.PId);
                }

                purchase.Price += _context.Offers.FirstOrDefault(x => x.Type == inputModel.ticketClass).Price * (inputModel.numOfChild + inputModel.numOfAdult);

                String msg;
                String userId = _userManager.GetUserId(HttpContext.User);
                MailAdapter mailAdapter = new MailAdapter();

                msg = "Thank you for your ticket purchase. Here are the details < br /> ";
                msg = msg + "Flight: " + flight.Name + "<br />" + "Fllght NO : " + inputModel.flightInfo.FlightNo + " < br /> ";

                int counter = 0;
                while (counter < inputModel.numOfAdult + inputModel.numOfChild)
                {
                    Ticket tic = new Ticket();
                    tic.ProcessTime = DateTime.Now;
                    tic.EventId = inputModel.flightInfo.Id;
                    tic.OwnerId = _userManager.GetUserId(HttpContext.User);
                    tic.CheckIn = false;
                    tic.isChild = counter < inputModel.numOfAdult ? false : true;
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

                    msg = msg + "Ticket: " + "<br />" + tic.Id + "<br />" + "Seat: " + seat.Col + seat.Row + "< br /><br />";

                }

                if (inputModel.PId == default)
                    _context.Purchases.Add(purchase);
                else
                    _context.Purchases.Update(purchase);

                _context.SaveChanges();

                String to = _context.Users.Where(a => a.Id == userId).Select(a => a.Email).FirstOrDefault().ToString();
                mailAdapter.SendMail(_userManager.GetUserId(HttpContext.User), msg, to);

                if (inputModel.returnDate <= DateTime.Now)
                {
                    AutoCancelManager.AutoCancelManagerStatic.DeleteOverTime(purchase.Id, _context);
                    return RedirectToAction("Purchase", new { id = purchase.Id });
                }
                else
                {
                    return RedirectToAction("SearchAsPage", new { arr = flight.Route.DepartureAirport.AirportName, dest = flight.Route.ArrivalAirport.AirportName, date = inputModel.returnDate, numOfAdult = inputModel.numOfAdult, numOfChild = inputModel.numOfChild, ticketClass = inputModel.ticketClass });
                }
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

        [HttpGet]
        [Authorize]
        [RequireHttps]
        public IActionResult Purchase(int id, int countOfBaby)
        {
            var purchase = _context.Purchases.FirstOrDefault(x => x.Id == id);


            if (purchase == null)
            {
                return NotFound();
            }

            string OwnerId = _userManager.GetUserId(HttpContext.User);
            var usercard = _context.CreditCards.Where(x => x.OwnerId == OwnerId).ToList();
            var cardlist = new SelectList(usercard, "Id", "CardNumber");
            ViewData["CardId"] = cardlist;

            var useradd = _context.Addresses.Where(x => x.OwnerId == OwnerId).ToList();
            var addlist = new SelectList(useradd, "Id", "Name");
            ViewData["Addresses"] = addlist;


            var ticket = purchase.Tickets.FirstOrDefault(x => x.OwnerId == OwnerId);
            int ticketid = ticket.Id;
            var offerticket = _context.OfferTickets.Where(x => x.TicketId == ticketid).ToList();
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

                /*
                int offerid = offerticket.OfferId;
                var offer = _context.Offers.FirstOrDefault(x => x.Id == offerid);
                string offer1 = offer.Description;
                ViewData["Offer"] = offer1;*/
            }
            else
            {
                ViewData["Offer"] = "No offer selected.";
            }

            if (countOfBaby != 0)
            {
                ViewData["Baby"] = countOfBaby;
            }
            else { ViewData["Baby"] = 0; }


            String name = _context.Users.Where(a => a.Id == _userManager.GetUserId(HttpContext.User)).Select(a => a.Name).FirstOrDefault().ToString() + " " +
                _context.Users.Where(a => a.Id == _userManager.GetUserId(HttpContext.User)).Select(a => a.Surname).FirstOrDefault().ToString();
            ViewData["Name"] = name;
            String email = _context.Users.Where(a => a.Id == _userManager.GetUserId(HttpContext.User)).Select(a => a.Email).FirstOrDefault().ToString();
            ViewData["Email"] = email;

            return View(purchase);
        }

        [HttpPost]
        [Authorize]
        [RequireHttps]
        [ValidateAntiForgeryToken]
        public IActionResult Purchase(int pId, string cardNumber, string cardExpiry, string cardCVC, string couponCode)
        {
            string OwnerId = _userManager.GetUserId(HttpContext.User);
            var usercard = _context.CreditCards.Where(x => x.OwnerId == OwnerId).ToList();
            var cardlist = new SelectList(usercard, "Id", "CardNumber");
            ViewData["CardId"] = cardlist;
            ViewData["Creditcards"] = "Select a card.";

            if(_context.Purchases.Find(pId) != null)
            {
                var purchase = _context.Purchases.FirstOrDefault(x => x.Id == pId);
                purchase.IsProcessed = true;
                _context.Purchases.Update(purchase);
                _context.SaveChanges();
            }


            //------------------------------------Send Mail Start------------------------------------------//

            //String msg;
            //MailAdapter mailAdapter = new MailAdapter();

            //msg = "Thank you for your ticket purchase. Here are the details < br /> ";
            //msg = msg + "Fllght NO : " + inputModel.flightInfo.FlightNo + " < br /> ";

            //var flt = _context.Flights.Where(a => a.FlightNo.Equals(inputModel.flightInfo.FlightNo)).FirstOrDefault();
            //var tic = _context.Tickets.Where(a => a.EventId.Equals(flt.Id) && a.OwnerId.Equals(OwnerId)).ToList();

            //foreach(var t in tic)
            //    {
            //    var seat = _context.Seats.Where(a => a.FlightId.Equals(flt.FlightNo) && a.TicketId.Equals(t.Id)).FirstOrDefault();

            //    msg = msg + "Flight: " + flt.Name + "<br />" + "Ticket: " + "<br />" + t.Id + "<br />" + "Seat: " + seat.Col + seat.Row + "< br /><br />";
            //}


            //String to = _context.Users.Where(a => a.Id == OwnerId).Select(a => a.Email).FirstOrDefault().ToString();
            //mailAdapter.SendMail(_userManager.GetUserId(HttpContext.User), msg, to);

            //------------------------------------Send Mail End------------------------------------------//


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
