namespace FusePortal.Application.UseCases.Academic.Subjects.Exceptions
{
    public class SubjectNotFoundException : Exception
    {
        public SubjectNotFoundException() : base()
        {
        }

        public SubjectNotFoundException(string? message) : base(message)
        {
        }

        public SubjectNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
