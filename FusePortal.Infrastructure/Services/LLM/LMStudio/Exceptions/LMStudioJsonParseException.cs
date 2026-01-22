namespace FusePortal.Infrastructure.Services.LLM.LMStudio.Exceptions
{
    public class LMStudioJsonParseException : Exception
    {
        public LMStudioJsonParseException() : base()
        {
        }

        public LMStudioJsonParseException(string? message) : base(message)
        {
        }

        public LMStudioJsonParseException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
