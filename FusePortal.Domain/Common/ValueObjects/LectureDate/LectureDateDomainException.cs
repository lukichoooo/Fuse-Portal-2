using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Common.ValueObjects.LectureDate
{
    public class LectureDateDomainException : DomainException
    {
        public LectureDateDomainException(string message) : base(message)
        {
        }

        public LectureDateDomainException() : base()
        {
        }

        public LectureDateDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
