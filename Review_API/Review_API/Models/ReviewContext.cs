using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Review_API.Models
{
    public class ReviewContext: DbContext
    {
        public ReviewContext(DbContextOptions<ReviewContext> options) : base(options) { }

        public DbSet<ReviewTask_RA> ReviewItems { get; set; }
        
    }
}
