using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Identity.UserAggregate
{
    public class UserDomainException : DomainException
    {
        public UserDomainException(string message) : base(message)
        {
        }

        public UserDomainException() : base()
        {
        }

        public UserDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
