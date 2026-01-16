using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.Common.ValueObjects;
using FusePortal.Domain.Entities.ChatAggregate;
using FusePortal.Domain.Entities.FileEntityAggregate;
using FusePortal.Domain.Entities.SubjectAggregate;
using FusePortal.Domain.Entities.UniversityAggregate;
using FusePortal.Domain.Entities.UserAggregate.UserDomainEvents;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.UserAggregate;

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

    private readonly List<FileEntity> _fileEntities;
    public IReadOnlyCollection<FileEntity> FileEntities => _fileEntities.AsReadOnly();

    private readonly List<Subject> _subjects;
    public IReadOnlyCollection<Subject> Subjects => _subjects.AsReadOnly();

    private readonly List<Chat> _chats;
    public IReadOnlyCollection<Chat> Chats => _chats.AsReadOnly();


    public User(
        string name,
        string email,
        string passwordHash,
        Address address)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        Address = address ?? throw new ArgumentNullException(nameof(address));
        Role = RoleType.Student;

        AddDomainEvent(new UserRegisteredEvent(Id, Name, Email));
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
            throw new ArgumentException(nameof(newEmail));

        var oldEmail = Email;
        Email = newEmail;
        AddDomainEvent(new UserEmailChangedEvent(Id, oldEmail, newEmail));
    }

    public void AddUniversity(University uni)
    {
        if (_universities.Contains(uni)) return;

        _universities.Add(uni);
        AddDomainEvent(new UserAddedUniversityEvent(Id, uni));
    }

    public void RemoveUniversity(University uni)
    {
        if (!_universities.Contains(uni)) return;

        _universities.Add(uni);
        AddDomainEvent(new UserRemovedUniversityEvent(Id, uni));
    }


    private User() { } // EF
}
