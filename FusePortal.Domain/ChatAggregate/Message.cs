using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.FileEntityAggregate;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.ChatAggregate
{
    public class Message : Entity
    {
        [Required]
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


        public Message(string text, bool fromUser, int chatId)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            FromUser = fromUser;
            ChatId = chatId;
        }
    }
}
