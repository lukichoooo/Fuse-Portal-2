using FusePortal.Application.Interfaces.EventDispatcher;
using MediatR;

namespace FusePortal.Infrastructure.Messaging
{
    public class DomainEventDispatcher(IMediator mediator) : IDomainEventDispatcher
    {
        private readonly IMediator _mediator = mediator;

        public async Task DispatchAsync(
                IEnumerable<INotification> events,
                CancellationToken cancellationToken = default)
        {
            foreach (var e in events)
                await _mediator.Publish(e, cancellationToken);
        }
    }
}
