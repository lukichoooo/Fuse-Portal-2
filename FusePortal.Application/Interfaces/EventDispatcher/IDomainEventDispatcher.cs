using MediatR;

namespace FusePortal.Application.Interfaces.EventDispatcher
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(
                IEnumerable<INotification> events,
                CancellationToken cancellationToken = default);
    }
}
