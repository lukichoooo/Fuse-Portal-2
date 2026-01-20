using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Convo.ChatAggregate
{
    public class MessageFileDomainException : DomainException
    {
        public MessageFileDomainException(string message) : base(message)
        {
        }

        public MessageFileDomainException() : base()
        {
        }

        public MessageFileDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
