namespace FusePortal.Application.UseCases.Convo.Chats.Exceptions
{
    public class ChatNotFoundException : Exception
    {
        public ChatNotFoundException() : base()
        {
        }

        public ChatNotFoundException(string? message) : base(message)
        {
        }

        public ChatNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
