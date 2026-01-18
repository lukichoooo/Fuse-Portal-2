using FusePortal.Application.UseCases.Identity.Auth;
using FusePortal.Application.UseCases.Identity.Auth.LoginUser;
using FusePortal.Application.UseCases.Identity.Auth.RegisterUser;
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
                [FromBody] LoginUserCommand login)
            => Ok(await _sender.Send(login));

        [HttpPost("register")]
        public async Task<ActionResult<List<AuthResponse>>> Register(
                [FromBody] RegisterUserCommand register)
            => Ok(await _sender.Send(register));
    }
}
