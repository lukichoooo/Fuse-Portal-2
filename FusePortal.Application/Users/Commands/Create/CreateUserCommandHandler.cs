using Facet.Extensions;
using FusePortal.Domain.Common.ValueObjects;
using FusePortal.Domain.UserAggregate;
using MediatR;

namespace FusePortal.Application.Users.Commands.Create
{
    public class CreateUserCommandHandler(IUserRepo repo)
        : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUserRepo _repo = repo;

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var addressDto = request.Dto.Address!;

            var user = new User(
                    request.Dto.Name,
                    request.Dto.Email,
                    request.Dto.Password,
                    new Address(addressDto.Country, addressDto.City));

            var result = await _repo.CreateAsync(user);
            return result.ToFacet<User, UserDto>();
        }
    }
}
