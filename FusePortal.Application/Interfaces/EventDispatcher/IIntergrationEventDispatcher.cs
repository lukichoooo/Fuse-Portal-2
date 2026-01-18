using FusePortal.Application.Common.SeedWork;

namespace FusePortal.Application.Interfaces.EventDispatcher
{
    public interface IIntergrationEventDispatcher
    {
        Task DispatchAsync(
                IEnumerable<IIntergrationEvent> events,
                CancellationToken ct = default);
    }
}
