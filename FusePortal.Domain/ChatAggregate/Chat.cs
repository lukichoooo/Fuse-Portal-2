using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.SeedWork;
using FusePortal.Domain.UserAggregate;

namespace FusePortal.Domain.ChatAggregate
{
    public class Chat : Entity, IAggregateRoot
    {
        [Required]
        public string Name { get; private set; } = "New Chat";

        public string? LastResponseId { get; private set; }

        [Required]
        public int UserId { get; private set; }

        [Required]
        public User? User { get; private set; }

        private readonly List<Message> _messages;
        public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();

    }
}
