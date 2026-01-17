namespace FusePortal.Application.Unis.Exceptions
{
    public class UniNotFoundException : Exception
    {
        public UniNotFoundException() : base()
        {
        }

        public UniNotFoundException(string? message) : base(message)
        {
        }

        public UniNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
