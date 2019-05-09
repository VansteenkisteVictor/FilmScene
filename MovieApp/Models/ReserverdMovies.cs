using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class ReserverdMovies
    {
        public IEnumerable<Reservation> Reservation { get; set; }
        public List<MovieDetail> MovieDetail { get; set; }
    }
}
