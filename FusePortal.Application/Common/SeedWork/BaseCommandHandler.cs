using MediatR;

namespace FusePortal.Application.Common.SeedWork
{
    public abstract class BaseCommandHandler<TCommand, TResult> : IRequestHandler<TCommand, TResult>
        where TCommand : IRequest<TResult>
    {
        private readonly IUnitOfWork _uow;

        protected BaseCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<TResult> Handle(
                TCommand command,
                CancellationToken ct)
        {
            var result = await ExecuteAsync(command, ct);
            await _uow.CommitAsync(ct);
            return result;
        }

        protected abstract Task<TResult> ExecuteAsync(
                TCommand command,
                CancellationToken ct);
    }

    // Non Retruning Version
    public abstract class BaseCommandHandler<TCommand> : IRequestHandler<TCommand>
        where TCommand : IRequest
    {
        private readonly IUnitOfWork _uow;

        protected BaseCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(
                TCommand command,
                CancellationToken ct)
        {
            await ExecuteAsync(command, ct);
            await _uow.CommitAsync(ct);
        }

        protected abstract Task ExecuteAsync(
                TCommand command,
                CancellationToken ct);
    }


}
