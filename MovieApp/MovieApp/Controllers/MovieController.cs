using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Models;
using MovieApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovies_SQL movies_SQL;
        private readonly IMovies_HTTP movies_HTTP;



        public MovieController(IMovies_SQL movies_SQL, IMovies_HTTP movies_HTTP)
        {
            this.movies_SQL = movies_SQL;
            this.movies_HTTP = movies_HTTP;
        }

        public async Task<IActionResult> Index(string name = null, string comment = null, int score = 0,string username = null,string password = null, string password2 = null)
        {
            string MovieID = HttpContext.Session.GetString("MovieID");
            
            if (name != null && MovieID != null)
            {
                string Username = HttpContext.Session.GetString("LoggedIn");
                ReviewTask_RA review = new ReviewTask_RA();
                review.Id = new Guid();
                review.Name = name;
                review.Comment = comment;
                review.Score = score;
                review.MovieId = MovieID;
                Login login = await movies_HTTP.Login(Username);
                review.UserId = login.Id.ToString();
                await movies_HTTP.PostReview(review);
            }

            await LoginRegister(username, password, password2);
            string LoggedIn = HttpContext.Session.GetString("LoggedIn");

            if (LoggedIn != null)
            {
                var movies = await movies_HTTP.OnGet();
                return View(movies);
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }
        }

        public async Task<IActionResult> MovieDetail(string id)
        {
            string LoggedIn = HttpContext.Session.GetString("LoggedIn");

            if (LoggedIn != null)
            {

                if (id == null)
                {
                    return NotFound();
                }

                var movie_detail = await movies_HTTP.OnGetDetail(id);
                if (movie_detail == null)
                {
                    return NotFound();
                }
                HttpContext.Session.SetString("MovieID", movie_detail.imdbID);
                return View(movie_detail);
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }
        }

        public async Task LoginRegister(string username, string password, string password2 = null)
        {
            string LoginRegisterValue = HttpContext.Session.GetString("LoginRegisterValue");
            if (LoginRegisterValue == "Register")
            {
                if (username != null && password != null && password2 != null)
                {
                    if (password2 == password)
                    {
                        Login register = new Login();
                        register.Id = new Guid();
                        register.Username = username;
                        var PassSalt = Password(password);
                        register.Password = PassSalt.Item1;
                        register.Salt = PassSalt.Item2;
                        await movies_HTTP.Register(register);

                        HttpContext.Session.SetString("LoggedIn", register.Username.ToString());

                    }
                }
            }

            if (LoginRegisterValue == "Login")
            {

                if (username != null && password != null)
                {
                    Login login = await movies_HTTP.Login(username);
                    byte[] salt = login.Salt;
                    if (login.Password == PasswordWithSalt(password, salt) )
                    {
                        HttpContext.Session.SetString("LoggedIn", login.Username.ToString());
                    }
                }
            }
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
            string LoggedIn = HttpContext.Session.GetString("LoggedIn");

            if (LoggedIn != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }
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
            string LoggedIn = HttpContext.Session.GetString("LoggedIn");

            if (LoggedIn != null)
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
            else
            {
                return RedirectToAction(nameof(Login));
            }
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

        public async Task<IActionResult> Login()
        {

            HttpContext.Session.SetString("LoginRegisterValue", "Login");
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(IFormCollection collection)
        {
            try
            {

                return RedirectToAction(nameof(Login));
            }
            catch
            {
                return View();
            }

        }


        public async Task<IActionResult> Register()
        {
            HttpContext.Session.SetString("LoginRegisterValue", "Register");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(IFormCollection collection)
        {
            try
            {

                return RedirectToAction(nameof(Register));
            }
            catch
            {
                return View();
            }

        }

        public async Task<IActionResult> MyMovie()
        {

            string LoggedIn = HttpContext.Session.GetString("LoggedIn");

            if (LoggedIn != null)
            {
                Login login = await movies_HTTP.Login(LoggedIn);
                var review = await movies_HTTP.OnGetReviewUser(login.Id.ToString());
                List<MovieDetail> reviewed_movies = new List<MovieDetail>();
                for (int i = 0; i < review.Count(); i++)
                {
                    MovieDetail movies = await movies_HTTP.OnGetDetail(review.ElementAt(i).MovieId);
                    reviewed_movies.Add(movies);
                }
                BigViewModel bigViewModel = new BigViewModel();
                bigViewModel.MovieDetail = reviewed_movies;
                bigViewModel.ReviewTask_RA = review;
                return View(bigViewModel);
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }

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

            string LoggedIn = HttpContext.Session.GetString("LoggedIn");

            if (LoggedIn != null)
            {
                await movies_HTTP.Delete(id);
                return RedirectToAction(nameof(MyMovie));
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }

        }

        public async Task<IActionResult> Logout()
        {
            string LoggedIn = HttpContext.Session.GetString("LoggedIn");
            if (LoggedIn != null)
            {
                HttpContext.Session.Remove("LoggedIn");
                return RedirectToAction(nameof(Login));
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }
        }

        public async Task<IActionResult> Update(string id,int number,string comment,string score)
        {       
            string LoggedIn = HttpContext.Session.GetString("LoggedIn");


            if (LoggedIn != null)
            {
                string Number_review = HttpContext.Session.GetString("Number_review");
                if (Number_review != null)
                {
                    number = int.Parse(Number_review);
                }
                IEnumerable<ReviewTask_RA> review = await movies_HTTP.OnGetReviewUser(id);
                ReviewTask_RA reviewElement = review.ElementAt(number);

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
                    return RedirectToAction(nameof(MyMovie));
                }
                HttpContext.Session.SetString("Number_review", number.ToString());

                return View(reviewElement);
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }

        }




        public static (string,byte[]) Password(string password)
        {

            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return (hashed, salt);
        }

        public static string PasswordWithSalt(string password,byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashed;
        }




    }
}
