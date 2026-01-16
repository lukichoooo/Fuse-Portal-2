using FusePortal.Application.Auth;
using FusePortal.Application.Auth.LoginUser;
using FusePortal.Application.Auth.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FusePortal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;

        [HttpPost("login")]
        public async Task<ActionResult<List<AuthResponse>>> LoginUser(
                [FromBody] LoginRequest login)
            => Ok(await _sender.Send(new LoginUserCommand(login)));

        [HttpPost("register")]
        public async Task<ActionResult<List<AuthResponse>>> Register(
                [FromBody] RegisterRequest register)
            => Ok(await _sender.Send(new RegisterUserCommand(register)));
    }
}
