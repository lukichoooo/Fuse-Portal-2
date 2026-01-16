namespace FusePortal.Domain.SeedWork;

public abstract class Entity
{
    public int Id { get; protected set; }

    private readonly List<INotification> _domainEvents = [];
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(INotification eventItem) => _domainEvents.Add(eventItem);
    public void RemoveDomainEvent(INotification eventItem) => _domainEvents.Remove(eventItem);
    public void ClearDomainEvents() => _domainEvents.Clear();

    public bool IsTransient() => Id == default;

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType()) return false;
        if (ReferenceEquals(this, obj)) return true;

        Entity other = (Entity)obj;
        if (IsTransient() || other.IsTransient()) return false;
        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return IsTransient() ? base.GetHashCode() : Id.GetHashCode() ^ 31;
    }

    public static bool operator ==(Entity left, Entity right) => Equals(left, right);
    public static bool operator !=(Entity left, Entity right) => !Equals(left, right);
}
