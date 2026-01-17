using Facet.Extensions;
using FusePortal.Application.Users.Exceptions;
using FusePortal.Domain.Entities.UserAggregate;
using MediatR;

namespace FusePortal.Application.Users.Queries.GetUserWithUnisById
{
    public class GetUserWithUnisByIdQueryHandler(IUserRepo repo)
        : IRequestHandler<GetUserWithUnisByIdQuery, UserWithUniDto>
    {
        private readonly IUserRepo _repo = repo;

        public async Task<UserWithUniDto> Handle(GetUserWithUnisByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _repo.GetUserWithUnisByIdAsync(request.Id)
                ?? throw new UserNotFoundException($"User Not Found With Id={request.Id}");

            return user.ToFacet<User, UserWithUniDto>();
        }
    }
}
