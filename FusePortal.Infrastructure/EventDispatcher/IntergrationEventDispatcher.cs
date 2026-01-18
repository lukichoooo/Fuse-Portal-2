using FusePortal.Application.Common.SeedWork;
using FusePortal.Application.Interfaces.EventDispatcher;
using MediatR;

namespace FusePortal.Infrastructure.EventDispatcher
{
    public class IntergrationEventDispatcher(IMediator mediator) : IIntergrationEventDispatcher
    {
        private readonly IMediator _mediator = mediator;

        public async Task DispatchAsync(
                IEnumerable<IIntergrationEvent> events,
                CancellationToken cancellationToken = default)
        {
            foreach (var e in events)
                await _mediator.Publish(e, cancellationToken);
        }
    }
}
