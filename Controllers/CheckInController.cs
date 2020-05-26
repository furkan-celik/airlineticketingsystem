using System;
using System.Collections.Generic;
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
    public class CheckInController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<AppUser> _userManager;

        public CheckInController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> CheckIn(int? id)
        {
            ViewData["Err"] = "";
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(x => x.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);

        }

        [HttpPost]

        public async Task<IActionResult> CheckIn(int id, int type, int countOfSeats, int countOfChild, int countOfBaby)
        {
            var c_ticket = _context.Tickets.Find(id);
            c_ticket.CheckIn = true;
            _context.SaveChanges();

            return RedirectToAction(nameof(Successful));
        }

        //Successful
        public IActionResult Successful()
        {
            return View();
        }
    }
}