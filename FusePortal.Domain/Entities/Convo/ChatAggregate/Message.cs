using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FusePortal.Domain.Entities.Content.FileEntityAggregate;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Convo.ChatAggregate
{
    public class Message : Entity
    {
        [Key]
        [Required]
        public int CountNumber { get; private set; }

        [Required]
        public string Text { get; private set; } = "";

        [Required]
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        [Required]
        public bool FromUser { get; private set; }

        [Required]
        [ForeignKey(nameof(Chat))]
        public Guid ChatId { get; private set; }

        private readonly List<FileEntity> _files = [];
        public IReadOnlyCollection<FileEntity> Files => _files.AsReadOnly();


        public Message(string text, bool fromUser, Guid chatId)
        {
            Text = text ?? throw new ChatDomainException($"Field Can't be Null or Empty: {nameof(text)}");
            FromUser = fromUser;
            ChatId = chatId;
        }

        internal void AttachFile(FileEntity file)
        {
            if (_files.Contains(file))
                return;

            _files.Add(file);
        }

        internal void DetachFile(FileEntity file)
        {
            if (!_files.Contains(file))
                return;

            _files.Remove(file);
        }


        private Message() { } // EF
    }
}
