using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    
    public class FlightsController : Controller
    {
        private readonly ApplicationDbContext _context;


        public FlightsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Flights
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Flights.ToListAsync());
        //}
        
        public IActionResult Index(string arr,string dest)//, DateTime date)
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "CityName", "CityName");
            ViewData["Err"] = "";
            ViewData["Date"] = @DateTime.Now.ToString("yyyy-MM-dd");

            var flights = from selectList in _context.Flights
                    select selectList;

            if (!String.IsNullOrEmpty(arr) && !String.IsNullOrEmpty(dest))
            {
                //ViewData["Date"] = date.ToString("yyyy-MM-dd");
                if (string.Equals(arr, dest))
                {
                    ViewData["Err"] = "Departure and Arrival can't be the same. Please do another search.";

                }
                //else if(date != null){
                //    ViewData["Err"] = date.ToString("MM-dd-yyyy");

                //}
                else {

                    flights = flights.Where(selectList => selectList.Departure.Equals(dest) && selectList.Arrival.Equals(arr));// && selectList.ETA.Date.Equals(date.Date));
                }
                
            }
            return View(flights.ToList());
        }
       

        // GET: Flights/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .FirstOrDefaultAsync(m => m.FlightNo == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // GET: Flights/Create
        [HttpGet]
        [Authorize(Roles = "WebAdmin,CompAdmin")]
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "CityName", "CityName");
            return View();
        }

        // POST: Flights/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "WebAdmin,CompAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FlightNo,Departure,Arrival,ETA")] Flight flight)
        {
            if (flight.Departure != flight.Arrival)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(flight);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
           
                ViewData["CityId"] = new SelectList(_context.Cities, "CityName", "CityName");
            }
            return View(flight);
        }

        // GET: Flights/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            ViewData["Err"] = "";
            ViewData["CityId"] = new SelectList(_context.Cities, "CityName", "CityName");
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("FlightNo,Departure,Arrival,ETA")] Flight flight)
        {
            ViewData["Err"] = "";
            ViewData["CityId"] = new SelectList(_context.Cities, "CityName", "CityName");
            if (id != flight.FlightNo)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                if (flight.Arrival.Equals(flight.Departure))
                {
                    ViewData["Err"] = "Departure and Arrival can't be the same city";
                }
                else
                {
                    try
                    {
                        _context.Update(flight);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!FlightExists(flight.FlightNo))
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
            }
            return View(flight);
        }

        // GET: Flights/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .FirstOrDefaultAsync(m => m.FlightNo == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var flight = await _context.Flights.FindAsync(id);
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlightExists(string id)
        {
            return _context.Flights.Any(e => e.FlightNo == id);
        }
    }
}
