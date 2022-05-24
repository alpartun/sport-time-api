using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using sporttime4.Models;

namespace sporttime4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private readonly ApplicationSettings _appSettings;
        private AuthenticationContext _context;
        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IOptions<ApplicationSettings>appSettings, AuthenticationContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
            _context = context;
        }
        [HttpPost]
        [Route("Register")]
        //POST : api/User/Register
        public async Task<IActionResult> Register(UserModel model)
        {
            var user = new User()
            {
                FullName = model.FullName,
                Surname = model.Surname,
                UserName = model.UserName,
                Age = model.Age,
                E_Mail = model.E_Mail,
                ConfirmedE_Mail = model.ConfirmedE_Mail,
                Gender = model.Gender,
                Position = model.Position,
                Height = model.Height,
                Weight = model.Weight,
            };
            try
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                return Ok(result);
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }
        [HttpPost]
        [Route("Login")]
        //POST : /api/User/Login
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            var isPasswordTrue = await _userManager.CheckPasswordAsync(user, model.Password);
            if (user != null && isPasswordTrue)
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserName",user.UserName.ToString()),
                        new Claim("UserID",user.Id.ToString()),
                        new Claim("EMail", user.E_Mail.ToString()),
                    }),
                    Expires = DateTime.UtcNow.AddHours(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)),SecurityAlgorithms.HmacSha256Signature),
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);

                return Ok(new {token});
            }
            return BadRequest(new {message = "Username or Password is incorrect."});
        }
        [HttpPost]
        [Route("Edit")]
        // POST : : /api/User/Edit
        public async Task<IActionResult> Edit(User userModel)
        {
            User userUpdate = await _userManager.FindByIdAsync(userModel.Id);
            var isPasswordTrue = await _userManager.CheckPasswordAsync(userUpdate, userModel.Password);
            if (isPasswordTrue && userUpdate!=null)
            {
                userUpdate.FullName = userModel.FullName;
                userUpdate.Surname = userModel.Surname;
                userUpdate.UserName = userModel.UserName;
                userUpdate.E_Mail = userModel.E_Mail;
                userUpdate.ConfirmedE_Mail = userModel.ConfirmedE_Mail;
                userUpdate.Age = userModel.Age;
                userUpdate.Gender = userModel.Gender;
                userUpdate.Position = userModel.Position;
                userUpdate.Height = userModel.Height;
                userUpdate.Weight = userModel.Weight;
            }
            var x = await _userManager.UpdateAsync(userUpdate);
            if (x.Succeeded)
            {
                _context.SaveChangesAsync();
                return Ok(x);
            }
            return BadRequest(new{message = "Username or Password is incorrect or Username is already exists."});
        }
    }
    
}

