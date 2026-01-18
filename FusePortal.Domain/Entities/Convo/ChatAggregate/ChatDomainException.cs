using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Convo.ChatAggregate
{
    public class ChatDomainException : DomainException
    {
        public ChatDomainException(string message) : base(message)
        {
        }

        public ChatDomainException() : base()
        {
        }

        public ChatDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
