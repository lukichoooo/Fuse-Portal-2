namespace FusePortal.Infrastructure.Services.ExamGenerator.Exceptions
{
    public class ExamScoreParsingException : Exception
    {
        public ExamScoreParsingException() : base()
        {
        }

        public ExamScoreParsingException(string? message) : base(message)
        {
        }

        public ExamScoreParsingException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
