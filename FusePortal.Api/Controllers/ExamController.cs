using FusePortal.Api.Settings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FusePortal.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ExamController(
            ISender sender,
            IOptions<ControllerSettings> options) : ControllerBase
    {
        private readonly ControllerSettings _settings = options.Value;
        private readonly ISender _sender = sender;

        // [HttpGet("{id:guid}")]
        // public async Task<ActionResult<UniDto>> CreateUni(
        //         [FromRoute] Guid id)
        //     => Ok(await _sender.Send(new GetUniByIdQuery(id)));
        //
        // [HttpPost]
        // public async Task<ActionResult<UniDto>> CreateUni(
        //         [FromBody] CreateUniCommand createUniCommand)
        // {
        //     await _sender.Send(createUniCommand);
        //     return Ok();
        // }
    }
}
