namespace FusePortal.Domain.Entities.ChatAggregate
{
    public class MessageDomainException : ChatDomainException
    {
        public MessageDomainException(string message) : base(message)
        {
        }

        public MessageDomainException() : base()
        {
        }

        public MessageDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
