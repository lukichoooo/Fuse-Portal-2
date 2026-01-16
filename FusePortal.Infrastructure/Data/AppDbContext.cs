using System.Reflection;
using FusePortal.Domain.ChatAggregate;
using FusePortal.Domain.FileEntityAggregate;
using FusePortal.Domain.SubjectAggregate;
using FusePortal.Domain.UniversityAggregate;
using FusePortal.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace FusePortal.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Subject> Exams { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<FileEntity> FileEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

