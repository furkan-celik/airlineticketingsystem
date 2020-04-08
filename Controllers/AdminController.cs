using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "WebAdmin")]
    public class AdminController : Controller
    {

        private readonly RoleManager<AppRole> roleManager;

        public AdminController(RoleManager<AppRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppRole role)
        {
            var roleExist = await roleManager.RoleExistsAsync(role.Name);

            if (!roleExist)
            {
                var result = await roleManager.CreateAsync(role);
            }
            return View();
        }
    }
}