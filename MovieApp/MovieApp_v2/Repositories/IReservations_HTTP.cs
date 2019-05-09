using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace MovieApp_v2.Repositories
{
    public interface IReservations_HTTP
    {
        IEnumerable<Reservation> Reservations { get; }

        Task<IEnumerable<Reservation>> GetReservationAll(string id);
        Task<IEnumerable<Reservation>> GetReservationUser(string id);
        Task PostReservation(Reservation reservation);
    }
}