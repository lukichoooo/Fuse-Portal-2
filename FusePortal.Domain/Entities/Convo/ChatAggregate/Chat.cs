using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FusePortal.Domain.Common.Objects;
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
        [ForeignKey(nameof(User))]
        public Guid UserId { get; private set; }

        public string? LastResponseId { get; private set; }

        private readonly List<Message> _messages = [];
        public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();

        public Chat(string name, Guid userId)
        {
            Name = name ?? throw new ChatDomainException($"Field Cant be Null or Empty: {nameof(name)}");
            UserId = userId;
            LastResponseId = null;

            AddDomainEvent(new ChatCreatedEvent(Id));
        }


        public void SendMessage(string messageText, List<FileData>? Files = null)
        {
            var message = new Message(messageText, fromUser: true, Id);

            foreach (var fileE in Files ?? [])
                message.AttachFile(fileE);

            _messages.Add(message);
            AddDomainEvent(new ChatMessageSentEvent(Id, message.Id));
        }

        public Message GetLastMessage()
        {
            if (_messages.Count == 0)
                throw new ChatDomainException($"Cannot get last message from empty chat. ChatId={Id}");
            return _messages.Last();
        }

        public void RecieveResponse(string responseText)
        {
            var response = new Message(responseText, fromUser: false, Id);

            _messages.Add(response);
            AddDomainEvent(new ChatResponseRecievedEvent(Id, response.Id));
        }

        public void RemoveMessage(Guid msgId)
        {
            var message = _messages.FirstOrDefault(m => m.Id == msgId)
                ?? throw new ChatDomainException($"Message with id={msgId} not found");

            _messages.Remove(message);
            AddDomainEvent(new ChatMessageRemovedEvent(Id, message.Id));
        }


        public void UpdateLastResponseId(string newLastResponseId)
        {
            LastResponseId = newLastResponseId;
        }


        private Chat() { } // EF
    }
}
