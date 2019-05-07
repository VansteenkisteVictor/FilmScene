using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Review_API.Data;
using Review_API.Models;

namespace Review_API.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MovieSearchController : Controller
    {
        private IDataProvider dataProvider;

        public MovieSearchController(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;

        }
        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<MovieSearch>> Get()
        {
            var movies = await dataProvider.GetAllMoviesAsync();
            return movies;
        }

        [HttpGet("{id}")]
        public async Task<MovieDetail> Get(string id)
        {
            var movies = await this.dataProvider.GetMovieDetailAsync(id);
            return movies;
        }
    }
}