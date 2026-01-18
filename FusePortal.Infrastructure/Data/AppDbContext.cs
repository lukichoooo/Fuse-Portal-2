using System.Reflection;
using FusePortal.Domain.Entities.Academic.ExamAggregate;
using FusePortal.Domain.Entities.Academic.SubjectAggregate;
using FusePortal.Domain.Entities.Academic.UniversityAggregate;
using FusePortal.Domain.Entities.Content.FileEntityAggregate;
using FusePortal.Domain.Entities.Convo.ChatAggregate;
using FusePortal.Domain.Entities.Identity.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace FusePortal.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        public DbSet<University> Universities { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Exam> Exams { get; set; }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DbSet<FileEntity> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

