using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Review_API.Data
{
    public interface IDataProvider
    {
        Task<ReviewTask_RA> Add(ReviewTask_RA review);
        Task AddLogin(Login login);
        Task<Reservation> AddReservation(Reservation reservation);
        Task Delete(string id);
        Task DeleteReservation(string id);
        Task<IEnumerable<Reservation>> GetAllReservationsASync();
        Task<IEnumerable<ReviewTask_RA>> GetAllReviewsASync(string MovieId);
        Task<IEnumerable<ReviewTask_RA>> GetAllReviewsASyncByUser(string UserId);
        Task<Reservation> GetDetailReservation(string id);
        Task<Login> GetLogin(string Username);
        Task<MovieDetail> GetMovieDetailAsync(string id);
        Task<IEnumerable<Reservation>> GetUserReservationsASync(string id);
        Task UpdateReservation(Reservation reservation);
        Task UpdateReview(ReviewTask_RA review);
    }
}