using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.SeedWork;
using FusePortal.Domain.UserAggregate;

namespace FusePortal.Domain.ChatAggregate
{
    public class Chat : Entity, IAggregateRoot
    {
        [Required]
        public required string Name { get; set; } = "New Chat";

        public string? LastResponseId { get; set; }

        [Required]
        public required int UserId { get; set; }

        [Required]
        public User? User { get; set; }

        public List<Message> Messages { get; set; } = [];
    }
}
