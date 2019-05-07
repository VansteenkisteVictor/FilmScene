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
                ReviewTask_RA review = new ReviewTask_RA();
                review.Id = new Guid();
                review.Name = name;
                review.Comment = comment;
                review.Score = score;
                review.MovieId = MovieID;
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
            

            //var movies = await movies_SQL.GetAllMoviesAsync();
            //return View(movies);
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

                        HttpContext.Session.SetString("LoggedIn", register.Id.ToString());

                    }
                }
            }

            if (LoginRegisterValue == "Login")
            {

                if (username != null && password != null)
                {
                    Login login = await movies_HTTP.Login(username);
                    byte[] salt = login.Salt;
                    string passwordtest = PasswordWithSalt(password, salt);
                    string passwordtest2 = PasswordWithSalt(password, salt);
                    if (login.Password == PasswordWithSalt(password, salt) )
                    {
                        HttpContext.Session.SetString("LoggedIn", login.Id.ToString());
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

                var movie_reviews = await //NOW DOING
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
