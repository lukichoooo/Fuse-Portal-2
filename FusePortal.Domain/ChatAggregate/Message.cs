using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.FileEntityAggregate;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.ChatAggregate
{
    public class Message : Entity
    {
        public string Text { get; private set; } = "";

        [Required]
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        [Required]
        public bool FromUser { get; private set; }

        [Required]
        public int ChatId { get; private set; }
        public Chat Chat { get; private set; }

        private readonly List<FileEntity> _files;
        public IReadOnlyCollection<FileEntity> Files => _files.AsReadOnly();
    }
}
