using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.Common.ValueObjects;
using FusePortal.Domain.SeedWork;
using FusePortal.Domain.UniversityAggregate.UniversityDomainEvents;
using FusePortal.Domain.UserAggregate;

namespace FusePortal.Domain.UniversityAggregate;

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
        Name = name;
        Address = address;

        AddDomainEvent(new UniversityCreatedEvent(Name, Address));
    }


    private University() { }
}
