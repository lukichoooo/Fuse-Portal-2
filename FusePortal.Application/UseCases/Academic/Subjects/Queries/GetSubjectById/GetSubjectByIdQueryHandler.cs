using Facet.Extensions;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.UseCases.Academic.Subjects.Exceptions;
using FusePortal.Domain.Entities.Academic.SubjectAggregate;
using MediatR;

namespace FusePortal.Application.UseCases.Academic.Subjects.Queries.GetSubjectById
{
    public class GetSubjectByIdQueryHandler : IRequestHandler<GetSubjectByIdQuery, SubjectDto>
    {
        private readonly ISubjectRepo _repo;
        private readonly IIdentityProvider _identity;

        public GetSubjectByIdQueryHandler(
                ISubjectRepo repo,
                IIdentityProvider identity
                )
        {
            _repo = repo;
            _identity = identity;
        }

        public async Task<SubjectDto> Handle(GetSubjectByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _identity.GetCurrentUserId();
            var subject = await _repo.GetByIdAsync(request.SubjectId, userId)
                ?? throw new SubjectNotFoundException($"Subject with Id={request.SubjectId} not found");
            return subject.ToFacet<Subject, SubjectDto>();
        }
    }
}
