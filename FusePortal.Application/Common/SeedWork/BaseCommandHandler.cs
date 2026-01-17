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
                TCommand request,
                CancellationToken cancellationToken)
        {
            var result = await ExecuteAsync(request, cancellationToken);
            await _uow.CommitAsync(cancellationToken);
            return result;
        }

        protected abstract Task<TResult> ExecuteAsync(
                TCommand request,
                CancellationToken cancellationToken);
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
                TCommand request,
                CancellationToken cancellationToken)
        {
            await ExecuteAsync(request, cancellationToken);
            await _uow.CommitAsync(cancellationToken);
        }

        protected abstract Task ExecuteAsync(
                TCommand request,
                CancellationToken cancellationToken);
    }


}
