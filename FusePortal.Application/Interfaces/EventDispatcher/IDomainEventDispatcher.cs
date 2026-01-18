using FusePortal.Domain.SeedWork;

namespace FusePortal.Application.Interfaces.EventDispatcher
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(
                IEnumerable<IDomainEvent> events,
                CancellationToken ct = default);
    }
}
