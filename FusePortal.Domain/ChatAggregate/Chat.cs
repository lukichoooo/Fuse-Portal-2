using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.SeedWork;
using FusePortal.Domain.UserAggregate;

namespace FusePortal.Domain.ChatAggregate
{
    public class Chat : Entity, IAggregateRoot
    {
        [Required]
        public string Name { get; private set; } = "New Chat";

        [Required]
        public int UserId { get; private set; }
        public User? User { get; private set; }

        public string? LastResponseId { get; private set; }

        private readonly List<Message> _messages;
        public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();

        public Chat(string name, int userId, string? lastResponseId = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            UserId = userId;
            LastResponseId = lastResponseId;
        }
    }
}
