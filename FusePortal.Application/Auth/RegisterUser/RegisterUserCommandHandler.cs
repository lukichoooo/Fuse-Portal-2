using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.Users.Exceptions;
using FusePortal.Domain.UserAggregate;
using MediatR;

namespace FusePortal.Application.Auth.RegisterUser
{
    public class RegisterUserCommandHandler(
            IUserRepo repo,
            IEncryptor encryptor,
            IJwtTokenGenerator jwt
            ) : IRequestHandler<RegisterUserCommand, AuthResponse>
    {
        private readonly IUserRepo _repo = repo;
        private readonly IEncryptor _encryptor = encryptor;
        private readonly IJwtTokenGenerator _jwt = jwt;

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

            await _repo.CreateAsync(user);
            return new AuthResponse(_jwt.GenerateToken(user), null!);
        }
    }
}
