namespace FusePortal.Application.Interfaces.Services
{
    public interface IMessageStreamer
    {
        Task StreamAsync(
                Guid chatId,
                string chunk,
                CancellationToken ct = default);
    }
}
