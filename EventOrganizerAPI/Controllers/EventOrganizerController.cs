using EventOrganizerAPI.Entities;
using EventOrganizerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventOrganizerAPI.Controllers
{
    [ApiController]
    [Route("api/event")]
    public class EventOrganizerController : ControllerBase
    {
        private readonly ILogger<EventOrganizerController> _logger;
        private readonly IEventOrganizerService _service;

        public EventOrganizerController(ILogger<EventOrganizerController> logger, IEventOrganizerService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Event>>> GetAll()
        {
            var events = await _service.GetAll();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetById([FromRoute] int id)
        {
            var eventById = await _service.GetById(id);
            return Ok(eventById);
        }
    }
}