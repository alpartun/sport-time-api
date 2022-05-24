using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using sporttime4.Models;

namespace sporttime4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private readonly ApplicationSettings _appSettings;
        private AuthenticationContext _context;
        public EventController(UserManager<User> userManager, SignInManager<User> signInManager,
            IOptions<ApplicationSettings> appSettings, AuthenticationContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
            _context = context;
        }
        [HttpPost]
        [Route("Create")]
        //POST : api/Event/Create
        public async Task<IActionResult> Create(EventModel model)
        {
            var event_ = new Event()
            {
                Name = model.Name,
                Description = model.Description,
                City = model.City,
                Location = model.Location,
                SportType = model.SportType,
                Size = model.Size,
                EstimateTime = model.EstimateTime,
                Amount = model.Amount,
                count = model.count,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                createdBy = model.createdBy,


            };
            if (model.Users != null)
            {
                event_.Users = model.Users;
            }
            try
            {
                var result = await _context.Events.AddAsync(event_);
                //_context.Events.Add(event_);
                await _context.SaveChangesAsync();
                return Ok(new{message="Success"});
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        [HttpGet]
        [Route("GetEvents")]
        public async Task<IActionResult> GetEvents()
        {
            var result =  _context.Events;
            return Ok(result);
        }

        [HttpGet]
        [Route("GetEvents/{Id:int:min(1)}")]
        public async Task<IActionResult> GetEvent(int Id)
        {
            if (Id != 0)
            {
                var result =  _context.Events.Where(s => s.Id == Id).ToList();
                
              

                return Ok(result);

            }

            return BadRequest(new {message="asdasd"});
        }
        [HttpPost]
        [Route("JoinEvent")]
        public async Task<IActionResult> JoinEvent(JoinModel joinModel)
        {

            Event updatedEvent = _context.Events.FirstOrDefault(c=>c.Id == joinModel.eventId);
            updatedEvent.count += 1;
            User user = await _userManager.FindByIdAsync(joinModel.userId);

            Participate participate = new Participate()
            {
                PEventId = updatedEvent.Id,
                PUserId = joinModel.userId,
            };

            try
            {
                var result = await _context.Participates.AddAsync(participate);
                await _context.SaveChangesAsync();
                return Ok(new {message = "Success"});
            }
            catch(Exception e)
            {
                return Ok(e);
            }



            _context.SaveChanges();
            return Ok(updatedEvent.count);
        }
    }
}
