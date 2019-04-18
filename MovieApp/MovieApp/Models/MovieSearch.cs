using System;
using System.Collections.Generic;
using System.Text;

namespace MovieApp.Models
{

    public class MovieSearch
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string imdbID { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
    }

}
