using FusePortal.Application.UseCases.Identity.Auth;
using Microsoft.AspNetCore.Mvc;

namespace FusePortal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController() : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<AuthResponse>>> GetStatus()
            => Ok();
    }
}

