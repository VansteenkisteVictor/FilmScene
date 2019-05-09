using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class BigViewModel
    {
        public IEnumerable<ReviewTask_RA> ReviewTask_RA { get; set; }
        public List<MovieDetail> MovieDetail { get; set; }
    }
}
