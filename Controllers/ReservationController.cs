using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Threading;


namespace WebApplication1.Controllers
{

    [Authorize(Roles = "User")]
    public class ReservationController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<AppUser> _userManager;

        public ReservationController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Cancel_Reservation(int? id)
        {
            ViewData["Err"] = "";
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Reservations
                .Include(x => x.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);

        }

        [HttpPost]
        public async Task<IActionResult> Cancel_Reservation(int id, int type, int countOfSeats, int countOfChild, int countOfBaby)
        {
            var Res_deleted = _context.Reservations.Find(id);

            foreach (var item in Res_deleted.Seats)
            {
                item.Availability = true;
                _context.Seats.Update(item);
            }

            _context.Reservations.Remove(Res_deleted);
            _context.SaveChanges();
            return RedirectToAction(nameof(Cancel));
        }

        [HttpGet]
        public async Task<IActionResult> Buy_After_Reservation(int? id)
        {
            ViewData["Err"] = "";
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Reservations
                .Include(x => x.Owner)
                .Include(x => x.Flight)
                .Include(x => x.Offers)
                .FirstOrDefaultAsync(m => m.Id == id);

            var resoffer = _context.ReservationOffers.Where(x => x.ReservationId == id).ToList();

            if (resoffer.Count != 0)
            {
                string offer1 = "";
                foreach (var item in resoffer)
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

            DateTime date = @event.Flight.Date;
            TimeSpan eta = @event.Flight.Route.ETA;

            var arrivaldate = date + eta;
            ViewData["ArrivalDate"] = arrivaldate;

            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);

        }

        [HttpPost]
        public async Task<IActionResult> Buy_After_Reservation(int id, int type, int countOfSeats, int countOfChild, int countOfBaby)
        {
            var seatList = _context.Seats.Where(a => a.ReservationId == id).ToList();
            var reservation = _context.Reservations.Find(id);

            countOfSeats = seatList.Count();

            int counter = 0;
            Purchase purchase = new Purchase() { IsProcessed = false, OwnerId = _userManager.GetUserId(HttpContext.User), Price = 0, ProcessTime = DateTime.Now };
            purchase.Tickets = new List<Ticket>();
            purchase.Price = 0;
            purchase.Price += _context.Offers.FirstOrDefault(x => x.Type == reservation.Seats.ToList()[0].TypeId).Price * (reservation.numOfChild + reservation.numOfAdult);

            foreach (var seatItem in reservation.Seats)
            {
                Ticket tic = new Ticket();
                tic.EventId = seatItem.FlightId;
                tic.ProcessTime = DateTime.Now;
                tic.OwnerId = _userManager.GetUserId(HttpContext.User);
                tic.CheckIn = false;
                tic.isChild = counter < reservation.numOfAdult ? false : true;
                _context.Tickets.Add(tic);
                await _context.SaveChangesAsync();

                int quantity = reservation.numOfAdult;
                int childQuantity = reservation.numOfChild;

                foreach (var item in reservation.Offers)
                {
                    if (!tic.isChild && quantity > 0)
                    {
                        OfferTicket tmp = new OfferTicket() { Offer = item.Offer, Ticket = tic };
                        _context.OfferTickets.Add(tmp);
                        if (_context.SaveChanges() > 0)
                        {
                            purchase.Price += tmp.Offer.Price;
                            quantity--;
                        }
                    }
                    else if (tic.isChild && childQuantity > 0)
                    {
                        OfferTicket tmp = new OfferTicket() { Offer = item.Offer, Ticket = tic };
                        _context.OfferTickets.Add(tmp);
                        if (_context.SaveChanges() > 0)
                        {
                            purchase.Price += tmp.Offer.ChildPrice;
                            childQuantity--;
                        }
                    }
                }

                Seat seat = seatList.ElementAt(counter);
                seat.TicketId = (int)tic.Id;
                seat.Availability = false;
                seat.ReservationId = null;
                _context.Update(seat);

                counter++;
                purchase.Tickets.Add(tic);
            }
            await _context.SaveChangesAsync();
            
            _context.Reservations.Remove(_context.Reservations.Find(id));
            _context.SaveChanges();

            _context.Purchases.Add(purchase);
            _context.SaveChanges();

            AutoCancelManager.AutoCancelManagerStatic.DeleteOverTime(purchase.Id, _context);
            return RedirectToAction("Purchase", "Flights", new { id = purchase.Id });
        }

        [HttpGet]
        public async Task<IActionResult> ReservationNow(int? id)
        {
            ViewData["Err"] = "";
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Flights
                .Include(x => x.Organizer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReservationNow(int id, int countOfBaby, FlightsController.InputModel inputModel)
        {
            var flight = await _context.Flights
         .Include(x => x.Organizer)
         .FirstOrDefaultAsync(m => m.Id == inputModel.flightInfo.Id);

            var seats = _context.Seats
                .Include(x => x.OfferType)
                .Where(x => x.FlightId == flight.Id).OrderBy(x => x.Row).ToList();

            var selectedOffers = flight.Offers.Select(x => x.Offer).ToList();

            List<Seat> selectedSeats = seats.Where(x => x.TypeId == inputModel.ticketClass && x.Availability && !(inputModel.seats[x.Col - 1][x.Row.ToCharArray()[0] - seats[0].Row.ToCharArray()[0]].Availability)).ToList();
            ViewData["Err"] = "";

            if (seats == null)
            {
                ViewData["Err"] = "There isn't any seat left in choosen class";
                var @event = await _context.Flights
             .Include(x => x.Organizer)
             .FirstOrDefaultAsync(m => m.Id == id);

                if (@event == null)
                {
                    return NotFound();
                }
                return View(@event);
            }
            else if (inputModel.numOfChild + inputModel.numOfAdult > 4)
            {
                ViewData["Err"] = "You can reserve at most 4 seats";
                var @event = await _context.Flights
              .Include(x => x.Organizer)
              .FirstOrDefaultAsync(m => m.Id == id);

                if (@event == null)
                {
                    return NotFound();
                }
                return View(@event);
            }
            else if (seats.Count < inputModel.numOfAdult + inputModel.numOfChild)
            {
                ViewData["Err"] = "There isn't enough seats for you to buy";
                var @event = await _context.Flights
              .Include(x => x.Organizer)
              .FirstOrDefaultAsync(m => m.Id == id);

                if (@event == null)
                {
                    return NotFound();
                }
                return View(@event);
            }
            else if (countOfBaby > inputModel.numOfAdult)
            {
                ViewData["Err"] = "Infants cannot be more than adults";
                var @event = await _context.Flights
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
                while (counter < inputModel.numOfAdult + inputModel.numOfChild)
                {
                    Reservation res = new Reservation();
                    res.FlightId = flight.Id;
                    res.OwnerId = _userManager.GetUserId(HttpContext.User);
                    res.processTime = DateTime.Now;
                    res.numOfAdult = inputModel.numOfAdult;
                    res.numOfChild = inputModel.numOfChild;
                    _context.Reservations.Add(res);
                    await _context.SaveChangesAsync();

                    foreach (var item in inputModel.offers)
                    {
                        if (item.quantity > 0)
                        {
                            ReservationOffer tmp = new ReservationOffer() { Offer = selectedOffers.Find(x => x.Id == item.offer.Id), Reservation = res };
                            _context.ReservationOffers.Add(tmp);
                            if (_context.SaveChanges() > 0)
                                item.quantity--;
                        }
                        else if (item.childQuantity > 0)
                        {
                            ReservationOffer tmp = new ReservationOffer() { Offer = selectedOffers.Find(x => x.Id == item.offer.Id), Reservation = res };
                            _context.ReservationOffers.Add(tmp);
                            if (_context.SaveChanges() > 0)
                                item.childQuantity--;
                        }
                    }

                    Seat seat = selectedSeats.ElementAt(counter);
                    seat.ReservationId = res.Id;
                    seat.Availability = false;
                    var tmp2 = _context.Seats.Update(seat);
                    try
                    {
                        await _context.SaveChangesAsync();
                        AutoCancelManager.AutoCancelManagerStatic.DeleteOverTime(res, _context);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        throw;
                    }

                    counter++;
                }

                return RedirectToAction(nameof(Successful));

            }

        }

        public static async Task<IActionResult> TimeoutAfter<IActionResult>(Task<IActionResult> task, TimeSpan timeout)
        {

            using (var timeoutCancellationTokenSource = new CancellationTokenSource())
            {
                var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
                if (completedTask == task)
                {
                    timeoutCancellationTokenSource.Cancel();
                    return await task;
                }
                else
                {
        
                   throw new TimeoutException("The reservation is incomplete due to the time.");
                }
            }
        }

        //Successful
        public IActionResult Successful()
        {
            return View();
        }

        //Cancel
        public IActionResult Cancel()
        {
            return View();
        }

    }
}