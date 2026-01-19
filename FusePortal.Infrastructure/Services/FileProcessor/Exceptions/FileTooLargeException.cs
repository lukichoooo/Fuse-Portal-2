namespace FusePortal.Infrastructure.Services.FileProcessor
{
    public class FileTooLargeException : Exception
    {
        public FileTooLargeException() : base()
        {
        }

        public FileTooLargeException(string? message) : base(message)
        {
        }

        public FileTooLargeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
