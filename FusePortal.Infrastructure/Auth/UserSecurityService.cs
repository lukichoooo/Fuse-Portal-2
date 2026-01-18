using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.UseCases.Identity.Users.Exceptions;
using FusePortal.Domain.Entities.Identity.UserAggregate;

namespace FusePortal.Infrastructure.Auth
{
    public class UserSecurityService(IEncryptor encryptor) : IUserSecurityService
    {
        private readonly IEncryptor _encryptor = encryptor;

        public string HashPassword(string password)
            => _encryptor.Encrypt(password);

        public void VerifyPassword(User user, string password)
        {
            if (_encryptor.Decrypt(user.PasswordHash) != password)
                throw new UserWrongCredentialsException("Wrong Credentials");
        }
    }
}
