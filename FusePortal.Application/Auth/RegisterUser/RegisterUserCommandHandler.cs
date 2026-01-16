using FusePortal.Application.Common;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.Users.Exceptions;
using FusePortal.Domain.Entities.UserAggregate;
using MediatR;

namespace FusePortal.Application.Auth.RegisterUser
{
    public class RegisterUserCommandHandler(
            IUserRepo repo,
            IEncryptor encryptor,
            IJwtTokenGenerator jwt,
            IUnitOfWork uow
            ) : IRequestHandler<RegisterUserCommand, AuthResponse>
    {
        private readonly IUserRepo _repo = repo;
        private readonly IEncryptor _encryptor = encryptor;
        private readonly IJwtTokenGenerator _jwt = jwt;
        private readonly IUnitOfWork _uow = uow;

        public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var dbuser = await _repo.GetByEmailAsync(request.Email);
            if (dbuser is not null)
                throw new UserAlreadyExistsException($"Email={request.Email} already in use.");

            var user = new User(
                    name: request.Name,
                    email: request.Email,
                    passwordHash: _encryptor.Encrypt(request.Password),
                    address: request.Address);

            await _repo.AddAsync(user);

            await _uow.CommitAsync(cancellationToken);

            return new AuthResponse(_jwt.GenerateToken(user), null!);
        }
    }
}
