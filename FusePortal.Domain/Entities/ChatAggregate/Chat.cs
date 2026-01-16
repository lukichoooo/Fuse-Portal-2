using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.Entities.UserAggregate;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.ChatAggregate
{
    public class Chat : Entity, IAggregateRoot
    {
        [Required]
        public string Name { get; private set; } = "New Chat";

        [Required]
        public Guid UserId { get; private set; }
        public User? User { get; private set; }

        public string? LastResponseId { get; private set; }

        private readonly List<Message> _messages;
        public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();

        public Chat(string name, Guid userId, string? lastResponseId = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            UserId = userId;
            LastResponseId = lastResponseId;
        }


        private Chat() { } // EF
    }
}
