using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.Common.ValueObjects.Address;
using FusePortal.Domain.Entities.UniversityAggregate.UniversityDomainEvents;
using FusePortal.Domain.Entities.UserAggregate;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.UniversityAggregate;

public class University : Entity, IAggregateRoot
{
    [Required]
    public string Name { get; private set; }

    [Required]
    public Address Address { get; private set; }

    private readonly List<User> _students;
    public IReadOnlyCollection<User> Students => _students.AsReadOnly();


    public University(string name, Address address)
    {
        Name = name ?? throw new UniversityDomainException($"field cant be null or empty: {nameof(name)}");
        Address = address ?? throw new UniversityDomainException($"field cant be null or empty: {nameof(address)}");

        AddDomainEvent(new UniversityCreatedEvent(Id, Name, Address));
    }

    private University() { } // EF
}
