using System.Security.Claims;
using FusePortal.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Http;

namespace FusePortal.Infrastructure.Auth
{
    public sealed class IdentityProvider(
            IHttpContextAccessor httpContextAccessor)
        : IIdentityProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public Guid GetCurrentUserId()
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("No active HttpContext.");

            var user = httpContext.User;
            if (user?.Identity?.IsAuthenticated != true)
                throw new UnauthorizedAccessException("User is not authenticated.");

            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("User id claim is missing.");

            if (!Guid.TryParse(idClaim.Value, out var userId))
                throw new UnauthorizedAccessException("User id claim is invalid.");

            return userId;
        }
    }

}
