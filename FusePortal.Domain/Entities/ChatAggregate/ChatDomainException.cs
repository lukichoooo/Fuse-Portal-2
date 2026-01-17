using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.ChatAggregate
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
