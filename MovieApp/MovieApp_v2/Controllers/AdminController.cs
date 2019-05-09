using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieApp_v2.Repositories;

namespace MovieApp_v2.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IMovies_HTTP movies_HTTP;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public AdminController(IMovies_HTTP movies_HTTP, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.movies_HTTP = movies_HTTP;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Users()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        public async Task<IActionResult> UserRoles(int id)
        {
            var user = _userManager.Users.ToList().ElementAt(id);

            HttpContext.Session.SetString("SelectedUserId", id.ToString());

            var roles = await _userManager.GetRolesAsync(user);
            return View(roles);
        }

        public async Task<IActionResult> AddToAdmin()
        {
            string id = HttpContext.Session.GetString("SelectedUserId");
            var user = _userManager.Users.ToList().ElementAt(int.Parse(id));

            bool x = await _roleManager.RoleExistsAsync("Admin");
            if (!x)
            {
                // first we create Admin role    
                var role = new IdentityRole();
                role.Name = "Admin";
                await _roleManager.CreateAsync(role);
            }
            var result1 = await _userManager.AddToRoleAsync(user, "Admin");
            return RedirectToAction(nameof(UserRoles));
        }

        public IActionResult AdminNav()
        {
            return View();
        }

    }
}