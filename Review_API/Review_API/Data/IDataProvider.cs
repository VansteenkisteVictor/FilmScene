using System.Collections.Generic;
using System.Threading.Tasks;
using Review_API.Models;

namespace Review_API.Data
{
    public interface IDataProvider
    {
        Task<ReviewTask_RA> Add(ReviewTask_RA review);
        Task AddLogin(Login login);
        Task<IEnumerable<MovieSearch>> GetAllMoviesAsync();
        Task<IEnumerable<ReviewTask_RA>> GetAllReviewsASync(string MovieId);
        Task<Login> GetLogin(string Password);
        Task<MovieDetail> GetMovieDetailAsync(string id);
    }
}