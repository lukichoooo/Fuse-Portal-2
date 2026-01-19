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
            // explicit relations

            modelBuilder.Entity<Chat>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .IsRequired();

            modelBuilder.Entity<FileEntity>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .IsRequired();

            modelBuilder.Entity<FileEntity>()
                  .HasOne<Message>()
                  .WithMany(m => m.Files)
                  .HasForeignKey(f => f.MessageId)
                  .HasPrincipalKey(m => m.Id);

            modelBuilder.Entity<Subject>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .IsRequired();

            modelBuilder.Entity<Exam>()
                  .HasOne<Subject>()
                  .WithMany()
                  .HasForeignKey(e => e.SubjectId)
                  .IsRequired();

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

