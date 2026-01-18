using FusePortal.Domain.SeedWork;
using MediatR;

namespace FusePortal.Application.Common.SeedWork
{
    public abstract class BaseDomainEventHandler<TNotification>
        : INotificationHandler<TNotification> where TNotification : IDomainEvent
    {
        public Task Handle(
                TNotification evt,
                CancellationToken ct)
            => ExecuteAsync(evt, ct);


        protected abstract Task ExecuteAsync(
                TNotification evt,
                CancellationToken ct);
    }
}
