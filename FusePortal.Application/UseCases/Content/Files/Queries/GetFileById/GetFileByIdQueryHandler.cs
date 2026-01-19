using Facet.Extensions;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Domain.Entities.Content.FileEntityAggregate;
using MediatR;

namespace FusePortal.Application.UseCases.Content.Files.Queries.GetFileById
{
    public class GetFileByIdQueryHandler
        : IRequestHandler<GetFileByIdQuery, FileDto>
    {
        private readonly IFileRepo _repo;
        private readonly IIdentityProvider _identity;

        public GetFileByIdQueryHandler(IFileRepo repo, IIdentityProvider identity)
        {
            _repo = repo;
            _identity = identity;
        }

        public async Task<FileDto> Handle(GetFileByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _identity.GetCurrentUserId();
            var fileE = await _repo.GetById(request.FileId, userId);
            if (fileE is null)
                throw new FileNotFoundException($"File Not Found With Id={request.FileId}");

            return fileE.ToFacet<FileEntity, FileDto>();
        }
    }
}
