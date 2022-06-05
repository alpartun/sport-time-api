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

            var check = _context.Participates.Where(c => c.PUserId == joinModel.userId);

            var flag = false;

            foreach (var item in check)
            {
                if (item.PEventId == joinModel.eventId)
                {
                    flag = true;
                    break;
                }
            }

            if (updatedEvent.Size == updatedEvent.count.ToString())
            {
                return Ok(new {message = "Event size is full."});
            }
            
            if (flag)
            {
                return Ok(new {message = "You already joined the event."});
            }
            else
            {

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
                    return Ok(updatedEvent.count);
                }
                catch (Exception e)
                {
                    return Ok(e);
                }


            }
        }

        [HttpPost]
        [Route("DisjoinEvent")]

        public async Task<IActionResult> DisjoinEvent(JoinModel model)
        {
            var updatedEvent =  _context.Events.FirstOrDefault(c => c.Id == model.eventId);
            var check =  _context.Participates.Where(c => c.PUserId == model.userId && c.PEventId == model.eventId).FirstOrDefault();
            if (check != null)
            {
                updatedEvent.count -= 1;
                _context.Participates.Attach(check);
                _context.Participates.Remove(check);
                await _context.SaveChangesAsync();
                return Ok(new {message="You succesfully disjoined the event."});
            }
            else
            {
                return Ok(new {message = "Error occurs."});

            }

        }
        /*
                [HttpPost]
                [Route("ButtonStatus")]

                public async Task<IActionResult> ButtonStatus(JoinModel model)
                {
                    var participateUser =  _context.Participates.Where(c => c.PUserId == model.userId);

                    foreach (var item in participateUser)
                    {
                        if (item.PEventId == model.eventId)
                        {
                            return Ok(new {message = "False"});
                        }
                    }

                    return Ok(new {message = "True"});

                }*/
        [HttpPost]
        [Route("ButtonStatus")]

        public async Task<IActionResult> ButtonStatus(JoinModel model)
        {

            var participateUser = _context.Participates.Where(c => c.PUserId == model.userId);
            var list = new List<string>();
            foreach (var item in participateUser)
            {
                list.Add(item.PEventId.ToString());
            }

            return Ok(list);


        }

        [HttpPost]
        [Route("ProfileEvents")]

        public async Task<IActionResult> ProfileEvents(JoinModel model)
        {
            var participates = _context.Participates.Where(c => c.PUserId == model.userId);
            var showEvents = new List<Event>();
            foreach (var item in participates)
            {
                var update = _context.Events.FirstOrDefault(c => c.Id == item.PEventId);
                showEvents.Add(update);

            }
            return Ok(showEvents);
        }

        [HttpPost]
        [Route("EditEvent")]
        public async  Task<IActionResult> EditEvent(Event eventModel)
        {
            Event updateEvent = await _context.Events.FindAsync(eventModel.Id);

            if (updateEvent != null)
            {
                updateEvent.Name = eventModel.Name;

                updateEvent.Amount = eventModel.Amount;

                updateEvent.City = eventModel.City;
                updateEvent.Description = eventModel.Description;
                updateEvent.StartDate = eventModel.EndDate;
                updateEvent.Location = eventModel.Location;
                updateEvent.EstimateTime = eventModel.EstimateTime;
                updateEvent.Size = eventModel.Size;
                updateEvent.SportType = eventModel.SportType;
                updateEvent.PhotoUrl = eventModel.PhotoUrl;


            }

            var x =  _context.Events.Update(updateEvent);

         
                await _context.SaveChangesAsync();
                return Ok(new {message="Event updated."});
        }


    }
}
