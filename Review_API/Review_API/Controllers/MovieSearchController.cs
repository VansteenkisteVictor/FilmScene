using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Review_API.Data;

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

        [HttpGet("{id}")]
        public async Task<MovieDetail> Get(string id)
        {
            var movies = await this.dataProvider.GetMovieDetailAsync(id);
            return movies;
        }
    }
}