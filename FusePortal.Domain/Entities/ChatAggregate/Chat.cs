using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.Entities.ChatAggregate.DomainEvents;
using FusePortal.Domain.Entities.FileEntityAggregate;
using FusePortal.Domain.Entities.UserAggregate;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.ChatAggregate
{
    public class Chat : Entity, IAggregateRoot
    {
        [Required]
        public string Name { get; private set; }

        [Required]
        public Guid UserId { get; private set; }
        public User? User { get; private set; }

        public string? LastResponseId { get; private set; }

        private readonly List<Message> _messages = [];
        public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();

        public Chat(string name, Guid userId)
        {
            Name = name ?? throw new ChatDomainException($"Field Cant be Null or Empty: {nameof(name)}");
            UserId = userId;
            LastResponseId = null;

            AddDomainEvent(new ChatCreatedEvent(Id, userId));
        }


        public void AddMessage(Message message)
        {
            _messages.Add(message);
            AddDomainEvent(new ChatMessageSentEvent(Id, message.Id));
        }

        public void RemoveMessage(Message message)
        {
            _messages.Remove(message);
            AddDomainEvent(new ChatMessageRemovedEvent(Id, message.Id));
        }

        public void AttachFileToMessage(Guid msgId, FileEntity file)
        {
            var message = _messages.FirstOrDefault(m => m.Id == msgId)
                ?? throw new ChatDomainException($"Message with id={msgId} not found");

            message.AttachFile(file);
            AddDomainEvent(new ChatMessageFileAttachedEvent(Id, message.Id, file.Id));
        }

        public void DetachFileFromMessage(Guid msgId, FileEntity file)
        {
            var message = _messages.FirstOrDefault(m => m.Id == msgId)
                ?? throw new ChatDomainException($"Message with id={msgId} not found");

            message.DetachFile(file);
            AddDomainEvent(new ChatMessageFileDetachedEvent(Id, message.Id, file.Id));
        }


        private Chat() { } // EF
    }
}
