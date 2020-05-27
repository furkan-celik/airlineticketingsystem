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
                .FirstOrDefaultAsync(m => m.Id == id);

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

            countOfSeats = seatList.Count();

            int counter = 0;
            while (counter < countOfSeats + countOfChild)
            {


                Ticket tic = new Ticket();
                tic.EventId = id;
                tic.ProcessTime = DateTime.Now;
                tic.OwnerId = _userManager.GetUserId(HttpContext.User);
                _context.Tickets.Add(tic);

                await _context.SaveChangesAsync();

                Seat seat = seatList.ElementAt(counter);
                seat.TicketId = (int)tic.Id;
                seat.Availability = false;
                seat.ReservationId = null;
                _context.Update(seat);
                await _context.SaveChangesAsync();

                counter++;

            }

            return RedirectToAction(nameof(Successful));

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
                .Where(x => x.FlightId == flight.Id).ToList();

            var selectedOffers = flight.Offers.Select(x => x.Offer).ToList();

            List<Seat> selectedSeats = seats.Where(x => x.Availability && !inputModel.seats[x.Col - 1][x.Row.ToCharArray()[0] - 'a'].Availability).ToList();
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
                    _context.Reservations.Add(res);
                    await _context.SaveChangesAsync();

                    foreach (var item in inputModel.offers)
                    {
                        if (!res.isChild && item.quantity > 0)
                        {
                            ReservationOffer tmp = new ReservationOffer() { Offer = selectedOffers.Find(x => x.Id == item.offer.Id), Reservation = res };
                            _context.ReservationOffers.Add(tmp);
                            if (_context.SaveChanges() > 0)
                                item.quantity--;
                        }
                        else if (res.isChild && item.childQuantity > 0)
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