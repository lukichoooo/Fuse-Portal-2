using FusePortal.Application.Common;
using FusePortal.Application.Interfaces.EventDispatcher;
using FusePortal.Domain.SeedWork;
using FusePortal.Infrastructure.Data;

namespace FusePortal.Infrastructure.Repo
{
    public sealed class EfUnitOfWork(
        AppDbContext context,
        IDomainEventDispatcher domainDispatcher) : IUnitOfWork
    {
        private readonly AppDbContext _context = context;
        private readonly IDomainEventDispatcher _domainDispatcher = domainDispatcher;

        public async Task CommitAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);

            var entities = _context.ChangeTracker
                .Entries<Entity>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity)
                .ToList();

            foreach (var entity in entities)
            {
                await _domainDispatcher.DispatchAsync(entity.DomainEvents, ct);
                entity.ClearDomainEvents();
            }
        }
    }

}
