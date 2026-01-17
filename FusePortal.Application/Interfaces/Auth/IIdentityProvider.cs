namespace FusePortal.Application.Interfaces.Auth
{
    public interface IIdentityProvider
    {
        public Guid GetCurrentUserId();
    }
}
