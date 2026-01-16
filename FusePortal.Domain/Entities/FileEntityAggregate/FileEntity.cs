using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.Entities.ChatAggregate;
using FusePortal.Domain.Entities.UserAggregate;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.FileEntityAggregate
{
    public class FileEntity : Entity, IAggregateRoot
    {
        [Required]
        public string Name { get; private set; }

        [Required]
        public string Text { get; private set; }

        [Required]
        public Guid UserId { get; private set; }
        public User? User { get; private set; }

        public Guid? MessageId { get; private set; }
        public Message? Message { get; private set; }

        public FileEntity(
                string name,
                string text,
                Guid userId,
                Guid? messageId = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Text = text ?? throw new ArgumentNullException(nameof(text));
            UserId = userId;
            MessageId = messageId;
        }


        private FileEntity() { } // EF
    }
}
