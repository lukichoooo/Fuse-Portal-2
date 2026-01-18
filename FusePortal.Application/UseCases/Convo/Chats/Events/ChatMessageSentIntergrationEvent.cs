using MediatR;

namespace FusePortal.Application.UseCases.Convo.Chats.Events
{
    public sealed record ChatMessageSentIntergrationEvent(
            Guid ChatId,
            Guid MessageId,
            bool Streaming = false) : INotification;
}
