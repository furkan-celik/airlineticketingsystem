﻿using System;
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
    [Authorize("ReqAdmin")]
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
        
        public IActionResult Index(string arr,string dep)//, DateTime date)
        {
            ViewData["AirportId"] = new SelectList(_context.Airports, "AirportName", "AirportName");
            ViewData["Err"] = "";
            //ViewData["Date"] = @DateTime.Now.ToString("yyyy-MM-dd");

            var route = from selectList in _context.Routes
                    select selectList;

            if (!String.IsNullOrEmpty(dep) && !String.IsNullOrEmpty(arr))
            {
                //ViewData["Date"] = date.ToString("yyyy-MM-dd");
                if (string.Equals(dep, arr))
                {
                    ViewData["Err"] = "Departure and Arrival can't be the same. Please do another search.";

                }
                //else if(date != null){
                //    ViewData["Err"] = date.ToString("MM-dd-yyyy");

                //}
                else {

                    route = route.Where(selectList => selectList.DepartureAirport.AirportName.Equals(dep) && selectList.ArrivalAirport.AirportName.Equals(arr));// && selectList.ETA.Date.Equals(date.Date));
                }
                
            }
            return View(route.ToList());
        }
       
        // GET: Routes/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Routes/Create
        [HttpGet]
        //[Authorize(Roles = "WebAdmin,CompAdmin")]
        public IActionResult Create()
        {
            //ViewData["CityId"] = new SelectList(_context.Cities, "CityName", "CityName");
            ViewData["AirportId"] = new SelectList(_context.Airports, "Id", "AirportName");
            return View();
        }

        // POST: Routes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "WebAdmin,CompAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Route route, bool twoWay)
        {
            ViewData["AirportId"] = new SelectList(_context.Airports, "Id", "AirportName");
            ViewData["Err"] = "";

           var routeIdCheck = await _context.Routes
                .FirstOrDefaultAsync(m => m.RouteId == route.RouteId);

            if(routeIdCheck != null)
            {
                ViewData["Err"] = "Give different Route Id";
                return View(route);
            }

            if (route.DepartureId != route.ArrivalId)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(route);
                    await _context.SaveChangesAsync();

                    if (twoWay)
                    {
                        route.RouteId = default;
                        route.DepartureAirport = null;
                        route.ArrivalAirport = null;
                        
                        int tmp = route.DepartureId;
                        route.DepartureId = route.ArrivalId;
                        route.ArrivalId = tmp;
                        _context.Routes.Add(route);
                    }

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                ViewData["Err"] = "Departure and Arrival can't be the same.";
                return View(route);
            }
            
            return View(route);
        }

        // GET: Routes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context.Routes.FindAsync(id);

            if (route == null)
            {
                return NotFound();
            }

            ViewData["Err"] = "";
            ViewData["AirportId"] = new SelectList(_context.Airports, "Id", "AirportName");
            return View(route);
        }

        // POST: Routes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int RouteId, [Bind("RouteId,DepartureId,ArrivalId,ETA")] Route route)
        {

            if (RouteId != route.RouteId)
            {
                return NotFound();
            }

            ViewData["Err"] = "";
            ViewData["AirportId"] = new SelectList(_context.Airports, "Id", "AirportName");

            if (ModelState.IsValid)
            {
                if (route.ArrivalId == route.DepartureId)
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

        // GET: Routes/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Routes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int RouteId)
        {
            var route = await _context.Routes.FindAsync(RouteId);
            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RouteExists(int id)
        {
            return _context.Routes.Any(e => e.RouteId == id);
        }
    }
}
