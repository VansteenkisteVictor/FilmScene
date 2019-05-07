using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Review_API.Data;
using Review_API.Models;

namespace Review_API.Controllers
{
    [Route("api/ReviewTask")]
    [ApiController]
    public class ReviewTaskController : Controller
    {
        private IDataProvider dataProvider;

        public ReviewTaskController(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;

        }

        // GET: api/Todo
        [HttpGet("{id}")]
        public async Task<IEnumerable<ReviewTask_RA>> Get(string id)
        {
            var reviews = await this.dataProvider.GetAllReviewsASync(id);
            return reviews;
        }

        [HttpGet("userreview/{id}")]
        public async Task<IEnumerable<ReviewTask_RA>> GetUserReview(string id)
        {
            var reviews = await this.dataProvider.GetAllReviewsASyncByUser(id);
            return reviews;
        }


        [HttpPost]
        public async Task Add([FromBody]ReviewTask_RA review)
        {
            await this.dataProvider.Add(review);
        }

        [HttpDelete("delete/{id}")]
        public async Task Delete(string id)
        {
            await this.dataProvider.Delete(id);
        }

        [HttpPut("change")]
        public async Task Update([FromBody]ReviewTask_RA review)
        {
            await this.dataProvider.UpdateReview(review);
        }
    }
}