using System.Reflection;
using FusePortal.Domain.UniversityAggregate;
using FusePortal.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace FusePortal.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<University> Universities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

