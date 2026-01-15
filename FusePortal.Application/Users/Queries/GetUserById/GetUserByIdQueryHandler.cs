using Facet.Extensions;
using FusePortal.Domain.UserAggregate;
using MediatR;

namespace FusePortal.Application.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler(IUserRepo repo)
        : IRequestHandler<GetUserByIdQuery, UserDetailsDto>
    {
        private readonly IUserRepo _repo = repo;

        public async Task<UserDetailsDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _repo.GetByIdAsync(request.Id);
            return user.ToFacet<User, UserDetailsDto>();
        }
    }
}
