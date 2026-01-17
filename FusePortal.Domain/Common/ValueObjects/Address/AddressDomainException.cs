using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Common.ValueObjects.Address
{
    public class AddressDomainException : DomainException
    {
        public AddressDomainException(string message) : base(message)
        {
        }

        public AddressDomainException() : base()
        {
        }

        public AddressDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
