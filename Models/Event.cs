using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace sporttime4.Models
{
    public class Event 
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public string SportType { get; set; }
        public string Size { get; set; }
        public string StartDate { get; set; }

        public string EndDate { get; set; }
        public string EstimateTime { get; set; }
        public string PhotoUrl { get; set; }

        public string Amount { get; set; }
        public string createdBy { get; set; }

        public int count { get; set; }

        public ICollection<Participate> participatedUserIds { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public Event()
        {
            this.Users = new List<User>();
        }


    }
}
