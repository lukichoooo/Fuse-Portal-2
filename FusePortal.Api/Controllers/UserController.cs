using FusePortal.Application.Users;
using FusePortal.Application.Users.Commands.Create;
using FusePortal.Application.Users.Queries.GetUserById;
using FusePortal.Application.Users.Queries.GetUsersPage;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FusePortal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;

        [HttpGet("all")]
        public async Task<ActionResult<List<UserDto>>> GetUsersPageAsync(
               [FromQuery] int? lastId,
               [FromQuery] int pageSize = 16)
            => Ok(await _sender.Send(new GetUsersPageQuery(lastId, pageSize)));

        [HttpGet("{id}")]
        public async Task<ActionResult<List<UserDto>>> CreateUser([FromRoute] int id)
            => Ok(await _sender.Send(new GetUserByIdQuery(id)));

        [HttpPost]
        public async Task<ActionResult<List<UserDto>>> CreateUser([FromBody] UserCreateDto dto)
            => Ok(await _sender.Send(new CreateUserCommand(dto)));

    }
}
