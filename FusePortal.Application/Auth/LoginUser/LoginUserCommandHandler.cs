using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.Users.Exceptions;
using FusePortal.Domain.Entities.UserAggregate;
using MediatR;

namespace FusePortal.Application.Auth.LoginUser
{
    public class LoginUserCommandHandler(
            IUserRepo repo,
            IEncryptor encryptor,
            IJwtTokenGenerator jwt
            ) : IRequestHandler<LoginUserCommand, AuthResponse>
    {
        private readonly IUserRepo _repo = repo;
        private readonly IEncryptor _encryptor = encryptor;
        private readonly IJwtTokenGenerator _jwt = jwt;

        public async Task<AuthResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _repo.GetByEmailAsync(request.Email)
                ?? throw new UserNotFoundException($"User not found with Email={request.Email}");

            if (_encryptor.Encrypt(request.Password) != user.PasswordHash)
                throw new UserWrongCredentialsException($"Wrong Password={request.Password}");

            return new AuthResponse(_jwt.GenerateToken(user), null!);
        }
    }
}
