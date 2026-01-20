namespace FusePortal.Application.UseCases.Academic.Exams.Exceptions
{
    public class ExamNotFoundException : Exception
    {
        public ExamNotFoundException() : base()
        {
        }

        public ExamNotFoundException(string? message) : base(message)
        {
        }

        public ExamNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
