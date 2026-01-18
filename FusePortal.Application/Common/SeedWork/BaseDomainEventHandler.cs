using MediatR;

namespace FusePortal.Application.Common.SeedWork
{
    public abstract class BaseDomainEventHandler<TNotification> : INotificationHandler<TNotification>
        where TNotification : INotification
    {
        private readonly IUnitOfWork _uow;

        protected BaseDomainEventHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(
                TNotification evt,
                CancellationToken ct)
        {
            await ExecuteAsync(evt, ct);
            await _uow.CommitAsync(ct);
        }

        protected abstract Task ExecuteAsync(
                TNotification evt,
                CancellationToken ct);
    }
}
