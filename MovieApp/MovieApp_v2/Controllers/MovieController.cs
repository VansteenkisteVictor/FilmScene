using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using MovieApp_v2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp_v2.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        private readonly IMovies_HTTP movies_HTTP;
        private readonly IomdbAPIMovies iomdbAPI;
        private readonly ILogger _logger;
        private UserManager<IdentityUser> _userManager;


        public MovieController(ILogger<MovieController> logger,IMovies_HTTP movies_HTTP,IomdbAPIMovies iomdbAPI, UserManager<IdentityUser> userManager)
        {
            this.movies_HTTP = movies_HTTP;
            this.iomdbAPI = iomdbAPI;
            _userManager = userManager;
            _logger = logger;


        }

        public async Task<IActionResult> Search(string comment = null, int score = 0)
        {
            string MovieID = HttpContext.Session.GetString("MovieID");
            var user = _userManager.GetUserAsync(User);
            if (comment != null && MovieID != null)
            {
                var Reviews = await movies_HTTP.OnGetReviewUser(user.Result.Id);

                for (int i = 0; i < Reviews.Count(); i++)
                {
                    if (Reviews.ElementAt(i).MovieId == MovieID)
                    {
                        HttpContext.Session.SetString("ReviewError", "You have already reviewed this movie");
                        return RedirectToAction(nameof(MovieReview));
                    }
                }
                ReviewTask_RA review = new ReviewTask_RA();
                review.Id = new Guid();
                review.Name = user.Result.Email;
                review.Comment = comment;
                review.Score = score;
                review.MovieId = MovieID;
                review.UserId = user.Result.Id;
                await movies_HTTP.PostReview(review);
                _logger.LogInformation("Added review to database");
            }
            string error = HttpContext.Session.GetString("ReviewError");
            error = HttpContext.Session.GetString("SearchError");
            HttpContext.Session.Remove("SearchError");
            _logger.LogInformation(error);
            return View("Search",error);

        }

        public async Task<IActionResult> Index(string search, string type, string year)
        {
            if(search != null)
            {
                var movies = await iomdbAPI.GetMovies(search,type,year);
                if (movies.Response == "True")
                {
                    _logger.LogInformation("Load movies");
                    return View(movies.Search);
                }
                HttpContext.Session.SetString("SearchError", "Something went wrong, try searching for something else");
                _logger.LogInformation("Failed to load movies");
                return RedirectToAction(nameof(Search));
            }
            else
            {
                HttpContext.Session.SetString("SearchError", "Fill something in");
                _logger.LogInformation("Nothing was filled in");
                return RedirectToAction(nameof(Search));
            }

        }

        public async Task<IActionResult> MovieDetail(string id)
        {
                if (id == null)
                {
                    _logger.LogInformation("Failed to get id from selected movie");
                return BadRequest();
                }

                var movie_detail = await iomdbAPI.GetMovieDetail(id);
                if (movie_detail == null)
                {
                    _logger.LogInformation("No corresponding movie found for id");
                    return NotFound();
                }
                HttpContext.Session.SetString("MovieID", movie_detail.imdbID);
               return View(movie_detail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MovieDetail(string id, IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                HttpContext.Session.SetString("MovieID", id);
                return RedirectToAction(nameof(MovieDetail));
            }
            catch
            {
                return View();
            }
        }

        public async Task<IActionResult> MovieReview()
        {
            string error = HttpContext.Session.GetString("ReviewError");
            _logger.LogInformation(error);
            return View("MovieReview",error);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MovieReview(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(MovieReview));
            }
            catch
            {
                return View();
            }
        }

        public async Task<IActionResult> AllReviews(string id)
        {

                if (id == null)
                {
                    return NotFound();
                }

                var movie_reviews = await movies_HTTP.OnGetReview(id);
                if (movie_reviews == null)
                {
                    return NotFound();
                }
                return View(movie_reviews);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AllReviews(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(AllReviews));
            }
            catch
            {
                return View();
            }
        }
        public async Task<IActionResult> MyMovie()
        {
            var user = _userManager.GetUserAsync(User);
            var review = await movies_HTTP.OnGetReviewUser(user.Result.Id);
            List<MovieDetail> reviewed_movies = new List<MovieDetail>();
            for (int i = 0; i < review.Count(); i++)
            {
                MovieDetail movies = await iomdbAPI.GetMovieDetail(review.ElementAt(i).MovieId);
                reviewed_movies.Add(movies);
            }
            BigViewModel bigViewModel = new BigViewModel();
            bigViewModel.MovieDetail = reviewed_movies;
            bigViewModel.ReviewTask_RA = review;
            return View(bigViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MyMovies(IFormCollection collection)
        {
            try
            {

                return RedirectToAction(nameof(MyMovies));
            }
            catch
            {
                return View();
            }

        }

        public async Task<IActionResult> Delete(string id)
        {
             _logger.LogInformation("Delete review "+id);
             await movies_HTTP.Delete(id);
             return RedirectToAction(nameof(MyMovie));

        }


        public async Task<IActionResult> Update(string id, int number, string comment, string score)
        {

                string Number_review = HttpContext.Session.GetString("Number_review");
                if (Number_review != null)
                {
                    
                    number = int.Parse(Number_review);
                }
                IEnumerable<ReviewTask_RA> review = await movies_HTTP.OnGetReviewUser(id);
                ReviewTask_RA reviewElement = review.ElementAt(number);
                _logger.LogInformation("Get selected review");

            if (comment != null)
                {
                    ReviewTask_RA newReviewElement = new ReviewTask_RA();
                    newReviewElement.Id = reviewElement.Id;
                    newReviewElement.Name = reviewElement.Name;
                    newReviewElement.Comment = comment;
                    newReviewElement.Score = int.Parse(score);
                    newReviewElement.UserId = reviewElement.UserId;
                    newReviewElement.MovieId = reviewElement.MovieId;

                    await movies_HTTP.Update(newReviewElement);
                    _logger.LogInformation("Update Review");
                    return RedirectToAction(nameof(MyMovie));
                }
                HttpContext.Session.SetString("Number_review", number.ToString());

                return View(reviewElement);


        }
    }
}
