using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.Users.Exceptions;
using FusePortal.Domain.UserAggregate;

namespace FusePortal.Infrastructure.Auth
{
    public class UserSecurityService(IEncryptor encryptor) : IUserSecurityService
    {
        private readonly IEncryptor _encryptor = encryptor;

        public void VerifyPassword(User user, string password)
        {
            if (_encryptor.Decrypt(user.PasswordHash) == password)
                throw new UserWrongCredentialsException("Wrong credemtoals");
        }
    }
}
