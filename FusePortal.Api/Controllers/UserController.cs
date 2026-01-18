using FusePortal.Api.Settings;
using FusePortal.Application.UseCases.Identity.Users;
using FusePortal.Application.UseCases.Identity.Users.Commands.AddUniToUser;
using FusePortal.Application.UseCases.Identity.Users.Commands.Delete;
using FusePortal.Application.UseCases.Identity.Users.Commands.RemoveUniFromUser;
using FusePortal.Application.UseCases.Identity.Users.Commands.Update;
using FusePortal.Application.UseCases.Identity.Users.Queries.GetUserById;
using FusePortal.Application.UseCases.Identity.Users.Queries.GetUsersPage;
using FusePortal.Application.UseCases.Identity.Users.Queries.GetUserWithUnisById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FusePortal.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
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
        public async Task<ActionResult<UserDto>> GetUser(
                [FromRoute] Guid id)
            => Ok(await _sender.Send(new GetUserByIdQuery(id)));

        [HttpGet("unis/{id:guid}")]
        public async Task<ActionResult<UserDto>> GetUserWithUni(
                [FromRoute] Guid id)
            => Ok(await _sender.Send(new GetUserWithUnisByIdQuery(id)));



        [HttpPut("unis/add/{uniId:guid}")]
        public async Task<IActionResult> AddUni(
                [FromRoute] Guid uniId)
        {
            await _sender.Send(new AddUniToUserCommand(uniId));
            return Ok();
        }

        [HttpPut("unis/remove/{uniId:guid}")]
        public async Task<IActionResult> RemoveUni(
                [FromRoute] Guid uniId)
        {
            await _sender.Send(new RemoveUniFromUserCommand(uniId));
            return Ok();
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateCurrentUserCredentialsAsync(
                [FromBody] UpdateUserCommand updateUserCommand
                )
        {
            await _sender.Send(updateUserCommand);
            return Ok();
        }

        [HttpDelete("me{id:guid}")]
        public async Task<IActionResult> DeleteCurrentUser(
                [FromRoute] Guid id
                )
        {
            await _sender.Send(new DeleteUserCommand(id));
            return Ok();
        }

        // [HttpGet("me")]
        // public async Task<ActionResult<UserPrivateDto>> GetCurrentUserPrivateDto()
        //     => Ok(await _service.GetCurrentUserPrivateDtoAsync());

    }
}
