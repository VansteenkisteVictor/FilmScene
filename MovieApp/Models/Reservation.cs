using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string Date { get; set; }
        public int Amount { get; set; }
        public string MovieId { get; set; }
        public string UserId { get; set; }
    }
}
