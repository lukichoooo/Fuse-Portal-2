using FusePortal.Application.Interfaces.EventDispatcher;
using FusePortal.Domain.SeedWork;
using MediatR;

namespace FusePortal.Infrastructure.EventDispatcher
{
    public class DomainEventDispatcher(IMediator mediator) : IDomainEventDispatcher
    {
        private readonly IMediator _mediator = mediator;

        public async Task DispatchAsync(
                IEnumerable<IDomainEvent> events,
                CancellationToken cancellationToken = default)
        {
            foreach (var e in events)
                await _mediator.Publish(e, cancellationToken);
        }
    }
}
