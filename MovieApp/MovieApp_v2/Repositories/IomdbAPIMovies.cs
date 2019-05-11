using System.Threading.Tasks;
using Models;

namespace MovieApp_v2.Repositories
{
    public interface IomdbAPIMovies
    {
        MovieDetail MovieDetail { get; set; }
        MovieSearch Movies { get; }

        Task<MovieDetail> GetMovieDetail(string id);
        Task<MovieSearch> GetMovies(string search, string type, string year);
    }
}