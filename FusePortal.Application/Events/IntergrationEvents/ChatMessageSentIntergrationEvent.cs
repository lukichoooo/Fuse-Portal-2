using FusePortal.Application.Common.SeedWork;

namespace FusePortal.Application.Events.IntergrationEvents
{
    public sealed record ChatMessageSentIntergrationEvent(
            Guid ChatId,
            Guid MessageId,
            bool Streaming) : IIntergrationEvent;
}
