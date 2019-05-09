using System.Collections.Generic;
using System.Threading.Tasks;
using Review_API.Models;

namespace Review_API.Data
{
    public interface IDataProvider
    {
        Task<ReviewTask_RA> Add(ReviewTask_RA review);
        Task AddLogin(Login login);
        Task Delete(string id);
        Task<IEnumerable<MovieSearch>> GetAllMoviesAsync();
        Task<IEnumerable<ReviewTask_RA>> GetAllReviewsASync(string MovieId);
        Task<IEnumerable<ReviewTask_RA>> GetAllReviewsASyncByUser(string UserId);
        Task<Login> GetLogin(string Username);
        Task<MovieDetail> GetMovieDetailAsync(string id);
        Task UpdateReview(ReviewTask_RA review);
    }
}