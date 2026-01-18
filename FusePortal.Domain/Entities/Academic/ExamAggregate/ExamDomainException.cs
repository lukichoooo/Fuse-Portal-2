using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Academic.ExamAggregate
{
    public class ExamDomainException : DomainException
    {
        public ExamDomainException(string message) : base(message)
        {
        }

        public ExamDomainException() : base()
        {
        }

        public ExamDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
