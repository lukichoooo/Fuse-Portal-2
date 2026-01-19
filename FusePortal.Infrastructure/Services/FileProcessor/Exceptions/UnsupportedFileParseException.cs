namespace FusePortal.Infrastructure.Services.FileProcessor.Exceptions
{
    public class UnsupportedFileParseException : Exception
    {
        public UnsupportedFileParseException() : base()
        {
        }

        public UnsupportedFileParseException(string? message) : base(message)
        {
        }

        public UnsupportedFileParseException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
