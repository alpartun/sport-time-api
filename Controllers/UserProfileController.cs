using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using sporttime4.Models;

namespace sporttime4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private UserManager<User> _userManager;
        public UserProfileController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        [Authorize]
        //GET : /api/UserProfile
        public async Task<IActionResult> GetUserProfile()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
            string userName = User.Claims.FirstOrDefault(c => c.Type == "UserName")?.Value;
            string userMail = User.Claims.FirstOrDefault(c => c.Type == "EMail")?.Value;

            var user = await _userManager.FindByNameAsync(userName);
            var user2 = await _userManager.FindByIdAsync(userId);

            return Ok(new
            {
                id = user2.Id,
                fullName = user2.FullName,
                surname = user2.Surname,
                userName = user2.UserName,
                password = user2.PasswordHash,
                e_Mail = user2.E_Mail,
                age = user2.Age,
                gender = user2.Gender,
                position = user2.Position,
                height = user2.Height,
                weight = user2.Weight
            });







        }
    }
}
