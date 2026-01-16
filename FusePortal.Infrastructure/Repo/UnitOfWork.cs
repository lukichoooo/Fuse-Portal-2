using FusePortal.Application.Common;
using FusePortal.Application.Interfaces.Messaging;
using FusePortal.Domain.SeedWork;
using FusePortal.Infrastructure.Data;

namespace FusePortal.Infrastructure.Repo
{
    public sealed class EfUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IDomainEventDispatcher _dispatcher;

        public EfUnitOfWork(
            AppDbContext context,
            IDomainEventDispatcher dispatcher)
        {
            _context = context;
            _dispatcher = dispatcher;
        }

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
                await _dispatcher.DispatchAsync(entity.DomainEvents, ct);
                entity.ClearDomainEvents();
            }
        }
    }

}
