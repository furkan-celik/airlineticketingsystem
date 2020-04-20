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

namespace WebApplication1.Controllers
{
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
        public IActionResult ReservationNow(int Id)
        {
            Reservation res = new Reservation();
            res.EventId = Id;
            res.OwnerId = _userManager.GetUserId(HttpContext.User);
            _context.Add(res);
            _context.SaveChanges();
            return View(res);

           
        }
    }
}