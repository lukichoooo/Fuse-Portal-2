using FusePortal.Application.Common.SeedWork;

namespace FusePortal.Application.Interfaces.EventDispatcher
{
    public interface IIntegrationEventDispatcher
    {
        Task DispatchAsync(
                IIntegrationEvent events,
                CancellationToken ct = default);
    }
}
