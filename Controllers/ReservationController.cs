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
            return RedirectToAction(nameof(Successful));
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
            var seatList = _context.Seats.Where(a =>a.ReservationId == id).ToList();

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
        public async Task<IActionResult> ReservationNow(int id, int type, int countOfSeats, int countOfChild, int countOfBaby)
        {
            var seatList = _context.Seats.Where(a => a.Availability == true && a.FlightId == id && a.TypeId == type).ToList();
            ViewData["Err"] = "";

            if (seatList == null)
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
            else if (countOfChild + countOfSeats > 4)
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
            else if (seatList.Count < countOfSeats + countOfChild)
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
            else if (countOfBaby > countOfSeats)
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
                while (counter < countOfSeats + countOfChild)
                {
                    Reservation res = new Reservation();
                    res.FlightId = id;
                    res.OwnerId = _userManager.GetUserId(HttpContext.User);
                    _context.Add(res);
                    await _context.SaveChangesAsync();

                    Seat seat = seatList.ElementAt(counter);
                    seat.ReservationId = res.Id;
                    seat.Availability = false;
                    _context.Update(seat);
                    await _context.SaveChangesAsync();

                    counter++;

                }


                return RedirectToAction(nameof(Successful));

            }

        }

        //Successful
        public IActionResult Successful()
        {
            return View();
        }

    }
}