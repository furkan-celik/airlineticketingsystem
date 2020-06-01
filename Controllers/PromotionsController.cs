using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Authorize("ReqAdmin")]
    public class PromotionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public PromotionsController(ApplicationDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public int? compid { get; set; }


        

        // GET: Promotions
        public async Task<IActionResult> Index()
        {

            var user = await _userManager.GetUserAsync(User);
            var mancompid = user.ManagingCompanyId;
            
            compid = mancompid;
            ViewData["Compi"] = user.ManagingCompany;
            if (mancompid == null)
            {
                var applicationDbContext = _context.Promotions.Include(p => p.Organizer);
                return View(await applicationDbContext.ToListAsync());
            }

            else
            {
                var applicationDbContext = _context.Promotions
                    .Where(t => t.Organizer.Id == mancompid)
                    .Include(p => p.Organizer);
                return View(await applicationDbContext.ToListAsync());
            }
            
        }

        // GET: Promotions/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotion = await _context.Promotions
                .Include(p => p.Organizer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (promotion == null)
            {
                return NotFound();
            }

            return View(promotion);
        }

        // GET: Promotions/Create
        public IActionResult Create(  Company compi  )
        {
            ViewData["TypeId"] = new SelectList(_context.OfferTypes, "Id", "Name");
            if (User.IsInRole("WebAdmin"))
            {
                ViewData["Company"] = new SelectList(_context.Companies, "Id", "Name");
            }
            else if (User.IsInRole("CompAdmin"))
            {
                var comp = _userManager.GetUserAsync(User).Result.ManagingCompany.Id;
                ViewData["Company"] = new SelectList(_context.Companies.Where(x => x.Id == comp), "Id", "Name");
            }
            return View();
        }

        // POST: Promotions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Discount,CompanyId")] Promotion promotion)
        {

            if (ModelState.IsValid)
            {
                _context.Add(promotion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            if (User.IsInRole("WebAdmin"))
            {
                ViewData["Company"] = new SelectList(_context.Companies, "Id", "Name");
            }
            else if (User.IsInRole("CompAdmin"))
            {
                var comp = _userManager.GetUserAsync(User).Result.ManagingCompany.Id;
                ViewData["Company"] = new SelectList(_context.Companies.Where(x => x.Id == comp), "Id", "Name");
            }
                return View(promotion);


            
        }

        // GET: Promotions/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotion = await _context.Promotions.FindAsync(id);
            if (promotion == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Description", promotion.CompanyId);
            return View(promotion);
        }

        // POST: Promotions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Discount,CompanyId")] Promotion promotion)
        {
            if (id != promotion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(promotion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PromotionExists(promotion.Id))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Description", promotion.CompanyId);
            return View(promotion);
        }

        // GET: Promotions/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotion = await _context.Promotions
                .Include(p => p.Organizer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (promotion == null)
            {
                return NotFound();
            }

            return View(promotion);
        }

        // POST: Promotions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var promotion = await _context.Promotions.FindAsync(id);
            _context.Promotions.Remove(promotion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PromotionExists(string id)
        {
            return _context.Promotions.Any(e => e.Id == id);
        }
    }
}
