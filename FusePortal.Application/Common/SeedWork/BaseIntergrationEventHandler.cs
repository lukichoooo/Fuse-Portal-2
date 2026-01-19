using MediatR;

namespace FusePortal.Application.Common.SeedWork
{
    /// <summary>
    /// Saves Changes to Database Automatically
    /// </summary>
    public abstract class BaseIntergrationEventHandler<TIntegrationEvent>
        : INotificationHandler<TIntegrationEvent>
        where TIntegrationEvent : IIntegrationEvent
    {
        private readonly IUnitOfWork _uow;

        protected BaseIntergrationEventHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(
                TIntegrationEvent evt,
                CancellationToken ct)
        {
            await ExecuteAsync(evt, ct);
            await _uow.CommitAsync(ct);
        }

        protected abstract Task ExecuteAsync(
                TIntegrationEvent evt,
                CancellationToken ct);
    }
}
