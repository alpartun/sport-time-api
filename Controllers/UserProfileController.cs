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
                id = user.Id,
                fullName = user.FullName,
                surname = user.Surname,
                userName = user.UserName,
                password = user.PasswordHash,
                e_Mail = user.E_Mail,
                age = user.Age,
                gender = user.Gender,
                position = user.Position,
                height = user.Height,
                weight = user.Weight
            });







        }
    }
}
