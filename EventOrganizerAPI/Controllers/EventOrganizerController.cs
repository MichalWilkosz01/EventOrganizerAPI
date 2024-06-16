using EventOrganizerAPI.Entities;
using EventOrganizerAPI.Models.Dto;
using EventOrganizerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventOrganizerAPI.Controllers
{
    [ApiController]
    [Route("api/event")]
    [Authorize]
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
        [AllowAnonymous]
        public async Task<ActionResult<Event>> GetById([FromRoute] int id)
        {
            var eventById = await _service.GetById(id);
            return Ok(eventById);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById([FromRoute] int id)
        {
            await _service.DeleteById(id);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> CreateEvent([FromBody] CreateEventDto dto)
        {
            var id = await _service.CreateEvent(dto);
            return Created($"/api/event/{id}", null);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEvent([FromBody] UpdateEventDto dto, [FromRoute] int id)
        {
            await _service.UpdateEvent(dto, id);
            return Ok();
        }

        [HttpPost("{eventId}/join")]
        public async Task<ActionResult> JoinEvent([FromRoute] int eventId)
        {
            await _service.Join(eventId);
            return Ok();
        }

        [HttpDelete("{eventId}/leave")]
        public async Task<ActionResult> LeaveEvent([FromRoute] int eventId)
        {
            await _service.Leave(eventId);
            return Ok();
        }
    }
}