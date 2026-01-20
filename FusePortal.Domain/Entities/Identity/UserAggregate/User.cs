using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.Common.ValueObjects.Address;
using FusePortal.Domain.Entities.Academic.UniversityAggregate;
using FusePortal.Domain.Entities.Identity.UserAggregate.DomainEvents;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Identity.UserAggregate;

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

    private readonly List<University> _universities = [];
    public IReadOnlyCollection<University> Universities => _universities.AsReadOnly();


    private readonly List<Guid> _subjectIds = [];

    private readonly List<Guid> _chatIds = [];



    public User(
        string name,
        string email,
        string passwordHash,
        Address address)
    {
        Name = name ?? throw new UserDomainException($"field can't be null or empty: {nameof(name)}");
        Email = email ?? throw new UserDomainException($"field can't be null or empty: {nameof(email)}");
        PasswordHash = passwordHash ?? throw new UserDomainException($"field can't be null or empty: {nameof(passwordHash)}");
        Address = address ?? throw new UserDomainException($"field can't be null or empty: {nameof(address)}");
        Role = RoleType.Student;

        AddDomainEvent(new UserRegisteredEvent(Id));
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
            throw new UserDomainException($"field cant be null or empty: {nameof(newEmail)}");

        var oldEmail = Email;
        Email = newEmail;
        AddDomainEvent(new UserEmailChangedEvent(Id, oldEmail, newEmail));
    }

    public void UpdatePasswordHash(string newPasswordHash)
    {
        if (PasswordHash == newPasswordHash)
            return;

        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new UserDomainException($"field cant be null or empty: {nameof(newPasswordHash)}");

        PasswordHash = newPasswordHash;
        AddDomainEvent(new UserPasswordChangedEvent(Id));
    }

    public void JoinUniversity(University uni)
    {
        if (_universities.Contains(uni)) return;

        _universities.Add(uni);
        AddDomainEvent(new UserJoinedUniversityEvent(Id, uni.Id));
    }

    public void LeaveUniversity(University uni)
    {
        if (!_universities.Contains(uni)) return;

        _universities.Remove(uni);
        AddDomainEvent(new UserLeftUniversityEvent(Id, uni.Id));
    }


    private User() { } // EF
}
