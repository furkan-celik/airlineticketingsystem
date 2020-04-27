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
    
    public class RoutesController : Controller
    {
        private readonly ApplicationDbContext _context;


        public RoutesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Flights
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Flights.ToListAsync());
        //}
        
        public IActionResult Index(string arr,string dest, DateTime date)
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "CityName", "CityName");
            ViewData["Err"] = "";
            ViewData["Date"] = @DateTime.Now.ToString("yyyy-MM-dd");

            var flights = from selectList in _context.Routes
                    select selectList;

            if (!String.IsNullOrEmpty(arr) && !String.IsNullOrEmpty(dest))
            {
                ViewData["Date"] = date.ToString("yyyy-MM-dd");
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
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var routes = await _context.Routes
                .FirstOrDefaultAsync(m => m.RouteId == id);
            if (routes == null)
            {
                return NotFound();
            }

            return View(routes);
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
        public async Task<IActionResult> Create([Bind("FlightNo,Departure,Arrival,ETA")] Route route)
        {
            if (route.Departure != route.Arrival)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(route);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
           
                ViewData["CityId"] = new SelectList(_context.Cities, "CityName", "CityName");
            }
            return View(route);
        }

        // GET: Flights/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Err"] = "";
            ViewData["CityId"] = new SelectList(_context.Cities, "CityName", "CityName");
            if (id == null)
            {
                return NotFound();
            }

            var routes = await _context.Routes.FindAsync(id);
            if (routes == null)
            {
                return NotFound();
            }
            return View(routes);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RouteId,Departure,Arrival,ETA")] Route route)
        {
            ViewData["Err"] = "";
            ViewData["CityId"] = new SelectList(_context.Cities, "CityName", "CityName");
            if (id != route.RouteId)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                if (route.Arrival.Equals(route.Departure))
                {
                    ViewData["Err"] = "Departure and Arrival can't be the same city";
                }
                else
                {
                    try
                    {
                        _context.Update(route);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!RouteExists(route.RouteId))
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
            return View(route);
        }

        // GET: Flights/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context.Routes
                .FirstOrDefaultAsync(m => m.RouteId == id);
            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flight = await _context.Routes.FindAsync(id);
            _context.Routes.Remove(flight);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RouteExists(int id)
        {
            return _context.Routes.Any(e => e.RouteId == id);
        }
    }
}
