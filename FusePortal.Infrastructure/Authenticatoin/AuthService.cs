using FusePortal.Application.Auth;
using FusePortal.Application.Interfaces;
using FusePortal.Application.Users.Exceptions;
using FusePortal.Domain.UserAggregate;

namespace FusePortal.Infrastructure.Authenticatoin
{
    public class AuthService(
            IUserRepo repo,
            IEncryptor encryptor,
            IJwtTokenGenerator jwt
            ) : IAuthService
    {
        private readonly IUserRepo _repo = repo;
        private readonly IEncryptor _encryptor = encryptor;
        private readonly IJwtTokenGenerator _jwt = jwt;

        public async Task<AuthResponse> LoginAsync(LoginRequest login)
        {
            var user = await _repo.GetByEmailAsync(login.Email)
                ?? throw new UserNotFoundException($"User not found with Email={login.Email}");

            if (_encryptor.Encrypt(login.Password) != user.PasswordHash)
                throw new UserWrongCredentialsException($"Wrong Password={login.Password}");

            return new AuthResponse(_jwt.GenerateToken(user), null!);
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest register)
        {
            var dbuser = await _repo.GetByEmailAsync(register.Email);
            if (dbuser is not null)
                throw new UserAlreadyExistsException($"Email={register.Email} already in use.");

            var user = new User(
                    name: register.Name,
                    email: register.Email,
                    passwordHash: _encryptor.Encrypt(register.Password),
                    address: register.Address);

            await _repo.CreateAsync(user);
            return new AuthResponse(_jwt.GenerateToken(user), null!);
        }
    }
}
