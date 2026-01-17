using System.Reflection;
using FusePortal.Domain.Entities.ChatAggregate;
using FusePortal.Domain.Entities.FileEntityAggregate;
using FusePortal.Domain.Entities.SubjectAggregate;
using FusePortal.Domain.Entities.UniversityAggregate;
using FusePortal.Domain.Entities.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace FusePortal.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Subject> Exams { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<FileEntity> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

