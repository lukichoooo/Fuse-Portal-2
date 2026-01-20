using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Convo.ChatAggregate
{
    public class MessageFile : Entity
    {
        [Required]
        public string Name { get; private set; }

        [Required]
        public string Text { get; private set; }

        [ForeignKey(nameof(Message))]
        public Guid MessageId { get; private set; }


        public MessageFile(
                string name,
                string text,
                Guid messageId)
        {
            Name = name ?? throw new MessageFileDomainException($"field cant be null or empty: {nameof(name)}");
            Text = text ?? throw new MessageFileDomainException($"field cant be null or empty: {nameof(text)}");
            MessageId = messageId;
        }


        private MessageFile() { } // EF
    }
}
