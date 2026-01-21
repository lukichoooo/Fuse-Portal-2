using FusePortal.Api.Controllers.Extensions;
using FusePortal.Api.Settings;
using FusePortal.Application.UseCases.Academic.Subjects.Commands.SavePortal;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FusePortal.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PortalController(
            ISender sender,
            IOptions<ControllerSettings> options) : ControllerBase
    {
        private readonly ControllerSettings _settings = options.Value;
        private readonly ISender _sender = sender;

        [HttpPost("upload-page")]
        public async Task<ActionResult> ParseHtmlPortalAsync()
        {
            var rawPage = await Request.GetRawBodyAsync();
            await _sender.Send(new SavePortalCommand(rawPage));
            return Ok();
        }

    }
}
