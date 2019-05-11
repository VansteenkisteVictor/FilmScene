using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace MovieApp_v2.Repositories
{
    public interface IReservations_HTTP
    {
        Reservation ReservationDetail { get; set; }
        IEnumerable<Reservation> Reservations { get; }

        Task Delete(string id);
        Task<IEnumerable<Reservation>> GetReservationAll();
        Task<Reservation> GetReservationDetail(string id);
        Task<IEnumerable<Reservation>> GetReservationUser(string id);
        Task PostReservation(Reservation reservation);
        Task UpdateReservations(Reservation reservation);
    }
}