using FusePortal.Application.Common.SeedWork;
using FusePortal.Application.Interfaces.EventDispatcher;
using MediatR;

namespace FusePortal.Infrastructure.EventDispatcher
{
    public class IntegrationEventDispatcher(IMediator mediator) : IIntegrationEventDispatcher
    {
        private readonly IMediator _mediator = mediator;

        public async Task DispatchAsync(
                IIntegrationEvent integrationEvent,
                CancellationToken cancellationToken = default
                )
            => await _mediator.Publish(integrationEvent, cancellationToken);
    }
}
