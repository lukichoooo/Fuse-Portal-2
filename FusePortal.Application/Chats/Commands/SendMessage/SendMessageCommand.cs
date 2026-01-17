using MediatR;

namespace FusePortal.Application.Chats.Commands.SendMessage
{
    public sealed record SendMessageCommand(
            Guid ChatId,
            string MessageText,
            List<Guid> FileIds
            ) : IRequest;

    // TODO: should add event and
    // LLMMessage event handler or something should catch it
    // and respond to it, once responded i will send new message to chat
    // 
    // LLM's response is also an event which gets caught and the
    // handler adds this message to the same chat entity as well
    // (just as a message from user)
}
