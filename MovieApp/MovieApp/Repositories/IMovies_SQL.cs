using System.Collections.Generic;
using System.Threading.Tasks;
using MovieApp.Models;

namespace MovieApp.Repositories
{
    public interface IMovies_SQL
    {
        Task Delete(int id);
        Task<IEnumerable<MovieSearch>> GetAllMoviesAsync();
        Task<MovieDetail> GetMovieDetailAsync(string id);
        Task<MovieSearch> Update(MovieSearch student);
    }
}