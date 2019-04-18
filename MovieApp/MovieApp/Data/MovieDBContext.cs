using Microsoft.EntityFrameworkCore;
using MovieApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Data
{
    public class MovieDBContext:DbContext
    {
        public MovieDBContext(DbContextOptions<MovieDBContext> options): base(options)
        {
        }
        public DbSet<MovieSearch> MovieSearch { get; set; }
        public DbSet<MovieDetail> MovieDetail { get; set; }
    }
}
