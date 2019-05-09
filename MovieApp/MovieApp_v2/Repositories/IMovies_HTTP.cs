using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace MovieApp_v2.Repositories
{
    public interface IMovies_HTTP
    {
        Login login { get; set; }
        IEnumerable<ReviewTask_RA> MovieReviews { get; }

        Task Delete(string id);
        Task<Login> Login(string Username);
        Task<IEnumerable<ReviewTask_RA>> OnGetReview(string id);
        Task<IEnumerable<ReviewTask_RA>> OnGetReviewUser(string id);
        Task PostReview(ReviewTask_RA review);
        Task Register(Login login);
        Task Update(ReviewTask_RA review);
    }
}