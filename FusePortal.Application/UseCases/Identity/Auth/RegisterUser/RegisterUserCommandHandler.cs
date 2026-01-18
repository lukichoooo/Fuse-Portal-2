using FusePortal.Application.Common;
using FusePortal.Application.Common.SeedWork;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.UseCases.Identity.Users.Exceptions;
using FusePortal.Domain.Entities.Identity.UserAggregate;

namespace FusePortal.Application.UseCases.Identity.Auth.RegisterUser
{
    public class RegisterUserCommandHandler : BaseCommandHandler<RegisterUserCommand, AuthResponse>
    {
        private readonly IUserRepo _repo;
        private readonly IEncryptor _encryptor;
        private readonly IJwtTokenGenerator _jwt;

        public RegisterUserCommandHandler(
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


        protected override async Task<AuthResponse> ExecuteAsync(RegisterUserCommand request, CancellationToken cancellationToken)
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
            return new AuthResponse(_jwt.GenerateToken(user), null!);
        }
    }
}
