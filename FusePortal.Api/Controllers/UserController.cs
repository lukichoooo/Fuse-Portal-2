using FusePortal.Api.Settings;
using FusePortal.Application.Users;
using FusePortal.Application.Users.Commands.Update;
using FusePortal.Application.Users.Queries.GetUserById;
using FusePortal.Application.Users.Queries.GetUsersPage;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FusePortal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(
            ISender sender,
            IOptions<ControllerSettings> options) : ControllerBase
    {
        private readonly ControllerSettings _settings = options.Value;
        private readonly ISender _sender = sender;

        [HttpGet("all")]
        public async Task<ActionResult<List<UserDto>>> GetUsersPageAsync(
               [FromQuery] Guid? lastId,
               [FromQuery] int? pageSize)
            => Ok(await _sender.Send(new GetUsersPageQuery(
                            lastId,
                            pageSize ?? _settings.DefaultPageSize)));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<List<UserDto>>> CreateUser(
                [FromRoute] Guid id)
            => Ok(await _sender.Send(new GetUserByIdQuery(id)));

        [HttpPut("me")]
        public async Task<ActionResult<UserDto>> UpdateCurrentUserCredentialsAsync(
                [FromBody] UpdateUserCommand updateUserCommand
                )
            => Ok(await _sender.Send(updateUserCommand));

        // [HttpGet("me")]
        // public async Task<ActionResult<UserPrivateDto>> GetCurrentUserPrivateDto()
        //     => Ok(await _service.GetCurrentUserPrivateDtoAsync());
        //
        // [HttpDelete("me")]
        // public async Task<ActionResult<UserDetailsDto>> DeleteCurrentUser()
        //     => Ok(await _service.DeleteCurrentUserAsync());
    }
}
