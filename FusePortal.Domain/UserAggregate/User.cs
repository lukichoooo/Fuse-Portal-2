using FusePortal.Domain.Common.ValueObjects;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.UserAggregate
{
    public sealed class User : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public RoleType Role { get; private set; }
        public Address Address { get; private set; }


        public User(
            string name,
            string email,
            string passwordHash,
            Address address)
        {
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Address = address;
            Role = RoleType.Student;

            // AddDomainEvent(new UserRegisteredEvent(Id, Email));
        }

        public void ChangeAddress(Address newAddress)
        {
            // TODO: idk if this is good
            if (string.IsNullOrWhiteSpace(newAddress.City))
                throw new ArgumentException("newAddress required");
            if (string.IsNullOrWhiteSpace(newAddress.City))
                throw new ArgumentException("City required");
            if (string.IsNullOrWhiteSpace(newAddress.Country))
                throw new ArgumentException("Country required");

            if (newAddress == Address)
                return;

            Address = newAddress;
            // AddDomainEvent(new UserRegisteredEvent(Id, Email));
        }

        public void ChangeRoleTo(RoleType role)
        {
            Role = role;
            // AddDomainEvent(new UserRegisteredEvent(Id, Email));
        }

        public void UpdateCredentials(string name, string email, string passwordHash)
        {
            // TODO: maybe improve domain validation or remove it
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name required");
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email required");
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("PasswordHash required");

            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            // AddDomainEvent(new UserRegisteredEvent(Id, Email));
        }


        private User() { } // EF
    }
}
