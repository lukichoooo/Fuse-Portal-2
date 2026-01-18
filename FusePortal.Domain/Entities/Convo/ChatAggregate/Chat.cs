using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.Entities.Content.FileEntityAggregate;
using FusePortal.Domain.Entities.Convo.ChatAggregate.DomainEvents;
using FusePortal.Domain.Entities.Identity.UserAggregate;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Convo.ChatAggregate
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


        public void SendMessage(Message message)
        {
            if (!message.FromUser)
                throw new ChatDomainException("Message must be from user");

            _messages.Add(message);
            AddDomainEvent(new ChatMessageSentEvent(Id, message.Id));
        }

        public void RecieveResponse(Message response)
        {
            if (response.FromUser)
                throw new ChatDomainException("Response cant be from user");

            _messages.Add(response);
            AddDomainEvent(new ChatResponseRecievedEvent(Id, response.Id));
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


        public void UpdateLastResponseId(string newLastResponseId)
        {
            LastResponseId = newLastResponseId;
            // AddDomainEvent();
        }


        private Chat() { } // EF
    }
}
