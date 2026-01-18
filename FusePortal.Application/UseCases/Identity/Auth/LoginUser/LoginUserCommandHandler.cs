using FusePortal.Application.Common;
using FusePortal.Application.Common.SeedWork;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.UseCases.Identity.Users.Exceptions;
using FusePortal.Domain.Entities.Identity.UserAggregate;

namespace FusePortal.Application.UseCases.Identity.Auth.LoginUser
{
    public class LoginUserCommandHandler : BaseCommandHandler<LoginUserCommand, AuthResponse>
    {
        private readonly IUserRepo _repo;
        private readonly IEncryptor _encryptor;
        private readonly IJwtTokenGenerator _jwt;

        public LoginUserCommandHandler(
                IUserRepo repo,
                IEncryptor encryptor,
                IJwtTokenGenerator jwt,
            IUnitOfWork uow
                ) : base(uow)
        {
            _repo = repo;
            _encryptor = encryptor;
            _jwt = jwt;
        }

        protected override async Task<AuthResponse> ExecuteAsync(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _repo.GetByEmailAsync(request.Email)
                ?? throw new UserNotFoundException($"User not found with Email={request.Email}");

            if (_encryptor.Encrypt(request.Password) != user.PasswordHash)
                throw new UserWrongCredentialsException($"Wrong Password={request.Password}");

            // TODO: add login event and refresh token
            return new AuthResponse(_jwt.GenerateToken(user), null!);
        }
    }
}
