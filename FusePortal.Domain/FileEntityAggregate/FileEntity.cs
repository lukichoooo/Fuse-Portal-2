using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.ChatAggregate;
using FusePortal.Domain.SeedWork;
using FusePortal.Domain.UserAggregate;

namespace FusePortal.Domain.FileEntityAggregate
{
    public class FileEntity : Entity, IAggregateRoot
    {
        [Required]
        public string Name { get; private set; }

        [Required]
        public string Text { get; private set; }

        [Required]
        public int UserId { get; private set; }
        public User? User { get; private set; }

        public int? MessageId { get; private set; }
        public Message? Message { get; private set; }
    }
}
