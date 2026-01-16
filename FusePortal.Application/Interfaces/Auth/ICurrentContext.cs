namespace FusePortal.Application.Interfaces.Auth
{
    public interface ICurrentContext
    {
        public Guid GetCurrentUserId();
    }
}
