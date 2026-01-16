using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.FileEntityAggregate;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.ChatAggregate
{
    public class Message : Entity
    {
        public string Text { get; set; } = "";

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public required int ChatId { get; set; }

        [Required]
        public required bool FromUser { get; set; }

        public Chat Chat { get; set; } = null!;

        public List<FileEntity> Files = [];
    }
}
