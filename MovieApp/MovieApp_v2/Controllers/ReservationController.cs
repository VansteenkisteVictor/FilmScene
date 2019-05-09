using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private UserManager<IdentityUser> _userManager;


        public ReservationController(IMovies_HTTP movies_HTTP, IomdbAPIMovies iomdbAPI, IReservations_HTTP reservations_HTTP, UserManager<IdentityUser> userManager)
        {
            this.movies_HTTP = movies_HTTP;
            this.iomdbAPI = iomdbAPI;
            this.reservations_HTTP = reservations_HTTP;
            _userManager = userManager;
        }

        public async Task<IActionResult> AddReservation(string location = null, string date = null, int amount = 0)
        {
            string MovieID = HttpContext.Session.GetString("MovieID");
            var user = _userManager.GetUserAsync(User);
            if (MovieID != null && location != null && date != null && amount != null)
            {
                Reservation reservation = new Reservation();
                reservation.Id = new Guid();
                reservation.Email = user.Result.Email;
                reservation.Location = location;
                reservation.Date = date;
                reservation.Amount = amount;
                reservation.MovieId = MovieID;
                reservation.UserId = user.Result.Id;
                await reservations_HTTP.PostReservation(reservation);

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
                
            
                return View(reserverdMovies);
            }
    }
}