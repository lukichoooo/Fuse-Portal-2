using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.ChatAggregate;
using FusePortal.Domain.SeedWork;
using FusePortal.Domain.UserAggregate;

namespace FusePortal.Domain.FileEntityAggregate
{
    public class FileEntity : Entity, IAggregateRoot
    {
        [Required]
        public required string Name { get; set; } = null!;

        [Required]
        public required string Text { get; set; } = null!;

        [Required]
        public required int UserId { get; set; }
        public User? User { get; set; }

        public int? MessageId { get; set; }
        public Message? Message { get; set; }
    }
}
