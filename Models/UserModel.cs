using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sporttime4.Models
{
    public class UserModel 
    {
        public string FullName { get; set; }

        public string Surname { get; set; }
        public string UserName { get; set; }

        public string E_Mail { get; set; }

        public string ConfirmedE_Mail { get; set; }

        public string Password { get; set; }

        public string ConfirmedPassword { get; set; }

        public string Age { get; set; }
        public string Gender { get; set; }

        public string Position { get; set; }

        public string Height { get; set; }
        public string Weight { get; set; }
    }
}
