using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.Common.ValueObjects;
using FusePortal.Domain.SeedWork;
using FusePortal.Domain.UniversityAggregate;
using FusePortal.Domain.UserAggregate.UserDomainEvents;

namespace FusePortal.Domain.UserAggregate;

public sealed class User : Entity, IAggregateRoot
{
    [Required]
    public string Name { get; private set; }

    [Required]
    public string Email { get; private set; }

    [Required]
    public string PasswordHash { get; private set; }

    [Required]
    public RoleType Role { get; private set; }

    [Required]
    public Address Address { get; private set; }

    private readonly List<University> _universities;
    public IReadOnlyCollection<University> Universities => _universities.AsReadOnly();


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

        AddDomainEvent(new UserRegisteredEvent(Name, Email, Address));
    }

    public void ChangeAddress(Address newAddress)
    {
        if (newAddress == Address)
            return;

        var oldAddress = Address;

        Address = newAddress;
        AddDomainEvent(new UserAddressChangedEvent(Id, oldAddress, newAddress));
    }

    public void ChangeRoleTo(RoleType newRole)
    {
        if (newRole == Role)
            return;

        var oldRole = Role;
        Role = newRole;
        AddDomainEvent(new UserRoleChangedEvent(Id, oldRole, newRole));
    }

    public void UpdateEmail(string newEmail)
    {
        if (Email == newEmail)
            return;

        if (string.IsNullOrWhiteSpace(newEmail))
            throw new ArgumentException("Email required");

        var oldEmail = Email;
        Email = newEmail;
        AddDomainEvent(new UserEmailChangedEvent(Id, oldEmail, newEmail));
    }

    public void AddUniversity(University uni)
    {
        if (Universities.Contains(uni)) return;

        Universities.Append(uni);
        AddDomainEvent(new UserAddedUniversityEvent(Id, uni));
    }

    public void RemoveUniversity(University uni)
    {
        if (!Universities.Contains(uni)) return;

        Universities.Append(uni);
        AddDomainEvent(new UserAddedUniversityEvent(Id, uni));
    }


    private User() { } // EF
}
