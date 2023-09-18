using EventOrganizerAPI.Models.Dto;
using EventOrganizerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventOrganizerAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterUserDto dto)
        {
            await _service.RegisterUser(dto);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUser([FromBody] LoginUserDto dto)
        {
            string token = await _service.GenerateJWT(dto);
            return Ok(token);
        }
    }
}
