using MediatR;

namespace FusePortal.Application.Common.SeedWork
{
    public abstract class BaseIntergrationEventHandler<TIntergrationEvent>
        : INotificationHandler<TIntergrationEvent>
        where TIntergrationEvent : IIntergrationEvent
    {
        private readonly IUnitOfWork _uow;

        protected BaseIntergrationEventHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(
                TIntergrationEvent evt,
                CancellationToken ct)
        {
            await ExecuteAsync(evt, ct);
            await _uow.CommitAsync(ct);
        }

        protected abstract Task ExecuteAsync(
                TIntergrationEvent evt,
                CancellationToken ct);
    }
}
