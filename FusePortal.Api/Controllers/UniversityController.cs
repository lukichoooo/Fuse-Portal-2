using FusePortal.Api.Settings;
using FusePortal.Application.UseCases.Academic.Unis;
using FusePortal.Application.UseCases.Academic.Unis.Commands.Create;
using FusePortal.Application.UseCases.Academic.Unis.Queries.GetUniById;
using FusePortal.Application.UseCases.Academic.Unis.Queries.GetUnisPage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FusePortal.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UniversityController(
            ISender sender,
            IOptions<ControllerSettings> options) : ControllerBase
    {
        private readonly ControllerSettings _settings = options.Value;
        private readonly ISender _sender = sender;

        [HttpGet("all")]
        public async Task<ActionResult<List<UniDto>>> GetUnisPageAsync(
               [FromQuery] Guid? lastId,
               [FromQuery] int? pageSize)
            => Ok(await _sender.Send(new GetUnisPageQuery(
                            lastId,
                            pageSize ?? _settings.DefaultPageSize)));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UniDto>> CreateUni(
                [FromRoute] Guid id)
            => Ok(await _sender.Send(new GetUniByIdQuery(id)));

        [HttpPost]
        public async Task<ActionResult<UniDto>> CreateUni(
                [FromBody] CreateUniCommand createUniCommand)
        {
            await _sender.Send(createUniCommand);
            return Ok();
        }
    }
}
