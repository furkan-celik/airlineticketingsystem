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
using System.Web;
using Microsoft.AspNetCore.Http;
using System.IO;


namespace WebApplication1.Controllers
{
    [Authorize(Roles = "WebAdmin")]
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;



        public CompaniesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Companies
        public IActionResult Index()
        {

            var companies = from selectList in _context.Companies
                            select selectList;
            return View(companies);
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            //await _context.Users.ForEachAsync((AppUser x) => { });

            if (company.Managers != null)
                ViewData["Managers"] = company.Managers.ToList<AppUser>();
            else
                ViewData["Managers"] = null;

            ViewData["Companies"] = new SelectList(_context.Companies, "Id", "Name");
            return View(company);
        }

        // GET: Companies/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,LogoLocation,Description,Type")] Company company, IFormFile file)
        {
            if (ModelState.IsValid)
            {


                //UploadFile(file, company.Id);
                var fileName = file.FileName;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                company.LogoLocation = fileName;

                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }



        public async void UploadFile(IFormFile file, int? id)
        {
            var fileName = file.FileName;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.Id == id);
            company.LogoLocation = fileName;
            _context.Update(company);

        }


        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,LogoLocation,Description,Type")] Company company)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
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
            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Companies/AddUser
        public IActionResult AddUser(int id)
        {
            HttpContext.Session.SetInt32("ManagingCompanyId", id);
            return Redirect("~/Identity/Account/Register");
        }

        // GET: Companies/MoveUser
        public IActionResult MoveUser(string id)
        {
            ViewData["Companies"] = new SelectList(_context.Companies, "Id", "Name");
            return PartialView(_context.Users.Find(id));
        }

        // Post: Companies/MoveUser
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MoveUser(string id, [Bind("Id, ManagingCompanyId")] AppUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }
            try
            {
                var mci = user.ManagingCompanyId;
                user = _context.Users.Find(id);
                user.ManagingCompanyId = mci;
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(user.ManagingCompanyId.Value))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Details", new { id = user.ManagingCompanyId });
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}
