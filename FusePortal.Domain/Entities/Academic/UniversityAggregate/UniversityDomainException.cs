using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Academic.UniversityAggregate
{
    public class UniversityDomainException : DomainException
    {
        public UniversityDomainException(string message) : base(message)
        {
        }

        public UniversityDomainException() : base()
        {
        }

        public UniversityDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
