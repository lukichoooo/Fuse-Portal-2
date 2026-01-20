using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Academic.SubjectAggregate
{
    public class SubjectDomainException : DomainException
    {
        public SubjectDomainException(string message) : base(message)
        {
        }

        public SubjectDomainException() : base()
        {
        }

        public SubjectDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
