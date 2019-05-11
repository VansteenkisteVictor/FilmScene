using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using MovieApp_v2.Repositories;

namespace MovieApp_v2.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IMovies_HTTP movies_HTTP;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IReservations_HTTP reservations_HTTP;
        private readonly ILogger _logger;

        public AdminController(ILogger<AdminController> logger,IMovies_HTTP movies_HTTP,IReservations_HTTP reservations_HTTP, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.movies_HTTP = movies_HTTP;
            _userManager = userManager;
            _roleManager = roleManager;
            this.reservations_HTTP = reservations_HTTP;
            _logger = logger;
        }

        public IActionResult Users()
        {
            var users = _userManager.Users.ToList();
            _logger.LogInformation("Get all users");
            return View(users);
        }

        public async Task<IActionResult> UserRoles(int id)
        {
            var user = _userManager.Users.ToList().ElementAt(id);

            HttpContext.Session.SetString("SelectedUserId", id.ToString());

            var roles = await _userManager.GetRolesAsync(user);
            _logger.LogInformation("Get user roles");
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
                _logger.LogInformation("Created role: " + role);
            }
            var result1 = await _userManager.AddToRoleAsync(user, "Admin");
            _logger.LogInformation("Made user: "+user+ "admin");
            return RedirectToAction(nameof(UserRoles));
        }

        public IActionResult AdminNav()
        {
            return View();
        }

        public async Task<IActionResult> AllReservations()
        {
            var reservations = await reservations_HTTP.GetReservationAll();
            if (reservations == null)
                return NotFound();
            return View(reservations);
        }

        public async Task<IActionResult> UpdateReservation(string id = null, string location = null, string date = null, string amount = null)
        {

            var reservation = await reservations_HTTP.GetReservationDetail(id);
            if(location != null && date != null && amount != null)
            {
                Reservation reservation_update = new Reservation();
                reservation_update.Id = reservation.Id;
                reservation_update.Email = reservation.Email;
                reservation_update.Location = location;
                reservation_update.Date = date;
                reservation_update.Amount = int.Parse(amount);
                reservation_update.MovieId = reservation.MovieId;
                reservation_update.UserId = reservation.UserId;
                await reservations_HTTP.UpdateReservations(reservation_update);
                _logger.LogInformation("Updated reservation");
                return RedirectToAction(nameof(AllReservations), "Admin");
            }
            else
            {
                return View(reservation);
            }
            
        }

        public async Task<IActionResult> DeleteReservation(string id)
        {   if (id == null)
                return BadRequest();
            await reservations_HTTP.Delete(id);
            _logger.LogInformation("Deleted reservation: "+id);
            return RedirectToAction(nameof(AllReservations), "Admin");
        }
    }
}