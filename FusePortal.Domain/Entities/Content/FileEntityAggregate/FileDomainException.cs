using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Content.FileEntityAggregate
{
    public class FileDomainException : DomainException
    {
        public FileDomainException(string message) : base(message)
        {
        }

        public FileDomainException() : base()
        {
        }

        public FileDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
