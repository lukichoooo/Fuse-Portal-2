using Facet.Extensions;
using FusePortal.Application.Common;
using FusePortal.Application.Users.Exceptions;
using FusePortal.Domain.Common.ValueObjects;
using FusePortal.Domain.UserAggregate;
using MediatR;

namespace FusePortal.Application.Users.Commands.Update
{
    public class UpdateUserCommandHandler(IUserRepo repo)
        : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly IUserRepo _repo = repo;

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {

            var user = await _repo.GetByIdAsync(request.Id)
                ?? throw new UserNotFoundException($"User With Id={request.Id}, Not Found");

            // TODO: passwords should be hashed
            user.UpdateCredentials(request.Name, request.Email, request.Password);
            user.ChangeAddress(request.Address.ToSource<AddressDto, Address>());

            await _repo.UpdateAsync(user);
            return user.ToFacet<User, UserDto>();
        }
    }
}
