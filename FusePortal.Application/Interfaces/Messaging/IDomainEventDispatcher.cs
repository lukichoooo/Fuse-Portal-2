
using MediatR;

namespace FusePortal.Application.Interfaces.Messaging
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(
                IEnumerable<INotification> events,
                CancellationToken cancellationToken = default);
    }
}
