using System;
using System.Collections.Generic;
using System.Linq;
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
    [Authorize]
    public class ReservationController : Controller
    {

        private readonly IMovies_HTTP movies_HTTP;
        private readonly IomdbAPIMovies iomdbAPI;
        private readonly IReservations_HTTP reservations_HTTP;
        private readonly ILogger _logger;
        private UserManager<IdentityUser> _userManager;


        public ReservationController(ILogger<Reservation> logger, IMovies_HTTP movies_HTTP, IomdbAPIMovies iomdbAPI, IReservations_HTTP reservations_HTTP, UserManager<IdentityUser> userManager)
        {
            this.movies_HTTP = movies_HTTP;
            this.iomdbAPI = iomdbAPI;
            this.reservations_HTTP = reservations_HTTP;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> AddReservation(string location = null, string date = null, int amount = 0)
        {
            string MovieID = HttpContext.Session.GetString("MovieID");
            var user = _userManager.GetUserAsync(User);
            if (MovieID != null && location != null && date != null && amount != 0)
            {
                var Reservations = await reservations_HTTP.GetReservationUser(user.Result.Id);
                _logger.LogInformation("Get user reveservations");
                for (int i = 0; i < Reservations.Count(); i++)
                {
                    if (Reservations.ElementAt(i).MovieId == MovieID)
                    {
                        return View("AddReservation", "You have already reviewed this movie");
                    }
                }

                Reservation reservation = new Reservation();
                reservation.Id = new Guid();
                reservation.Email = user.Result.Email;
                reservation.Location = location;
                reservation.Date = date;
                reservation.Amount = amount;
                reservation.MovieId = MovieID;
                reservation.UserId = user.Result.Id;
                await reservations_HTTP.PostReservation(reservation);
                _logger.LogInformation("Add reservation");
                return RedirectToAction(nameof(MyReservation),"Reservation");
            }
            else
            {
                return View();
            }
        }

            public async Task<IActionResult> MyReservation()
            {
                var id = _userManager.GetUserAsync(User).Result.Id;

                
                var myreservations = await reservations_HTTP.GetReservationUser(id.ToString());
                if (myreservations == null)
                {
                    _logger.LogInformation("No reservations found");
                    return NotFound();
                }
                List<MovieDetail> reservered_movies = new List<MovieDetail>();
                for (int i = 0; i < myreservations.Count(); i++)
                {
                    MovieDetail movies = await iomdbAPI.GetMovieDetail(myreservations.ElementAt(i).MovieId);
                    reservered_movies.Add(movies);
                }

                ReserverdMovies reserverdMovies = new ReserverdMovies();
                reserverdMovies.MovieDetail = reservered_movies;
                reserverdMovies.Reservation = myreservations;
                _logger.LogInformation("Load current user reservations");

            return View(reserverdMovies);
            }
    
        public async Task<IActionResult> DeleteReservation(string id)
        {
            if (id != null)
            {
                _logger.LogInformation("Failed to get id for reservations");
                return BadRequest();
            }
            await reservations_HTTP.Delete(id);
            _logger.LogInformation("Deleted reservation: " + id);
            return RedirectToAction(nameof(MyReservation), "Reservation");
        }
    }
}