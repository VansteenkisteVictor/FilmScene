using System.Collections.Generic;
using System.Threading.Tasks;
using MovieApp.Models;

namespace MovieApp.Repositories
{
    public interface IMovies_HTTP
    {
        Login login { get; set; }
        MovieDetail MovieDetail { get; set; }
        IEnumerable<ReviewTask_RA> MovieReviews { get; }
        IEnumerable<MovieSearch> Movies { get; }

        Task Delete(string id);
        Task<Login> Login(string Username);
        Task<IEnumerable<MovieSearch>> OnGet();
        Task<MovieDetail> OnGetDetail(string id);
        Task<IEnumerable<ReviewTask_RA>> OnGetReview(string id);
        Task<IEnumerable<ReviewTask_RA>> OnGetReviewUser(string id);
        Task PostReview(ReviewTask_RA review);
        Task Register(Login login);
        Task Update(ReviewTask_RA review);
    }
}