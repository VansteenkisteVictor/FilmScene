using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Review_API.Models
{
    public class ReviewTask_RA
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public int Score { get; set; }
        public string MovieId { get; set; }
        public string UserId { get; set; }
    }
}
