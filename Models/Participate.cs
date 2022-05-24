using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sporttime4.Models
{
    public class Participate
    {
        [Key]
        public int ParticipatedId { get; set; }
        public int PEventId { get; set; }
        public string PUserId { get; set; }
        public Event Event { get; set; }

    }
}
