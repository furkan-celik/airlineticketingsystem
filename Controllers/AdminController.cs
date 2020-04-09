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
    public class AdminController : Microsoft.AspNetCore.Mvc.Controller
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
        public async Task<IActionResult> CreateAsync()
        {
            string[] fixedRoles = new string[] { "WebAdmin", "UserAdmin", "User" };
            foreach (var item in fixedRoles)
            {
                if (!await roleManager.RoleExistsAsync(item))
                {
                    await roleManager.CreateAsync(new AppRole() { Name = item });
                }
            }

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