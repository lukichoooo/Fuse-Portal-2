using FusePortal.Application.Common;
using FusePortal.Application.Common.SeedWork;
using FusePortal.Domain.Entities.Academic.UniversityAggregate;

namespace FusePortal.Application.UseCases.Academic.Unis.Commands.Create
{
    public class CreateUniCommandHandler : BaseCommandHandler<CreateUniCommand>
    {
        private readonly IUniRepo _repo;

        public CreateUniCommandHandler(
            IUniRepo repo,
            IUnitOfWork uow)
            : base(uow)
        {
            _repo = repo;
        }

        protected override async Task ExecuteAsync(CreateUniCommand request, CancellationToken cancellationToken)
        {
            var uni = new University(request.Name, request.Address);
            await _repo.AddAsync(uni);
        }
    }
}
