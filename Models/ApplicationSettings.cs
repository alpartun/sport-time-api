using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sporttime4.Models
{
    public class ApplicationSettings : Controller
    {
        public string JWT_Secret { get; set; }
        public string Client_URL { get; set; }

    }
}
