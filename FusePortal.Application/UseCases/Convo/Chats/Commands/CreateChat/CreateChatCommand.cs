using MediatR;

namespace FusePortal.Application.UseCases.Convo.Chats.Commands.CreateChat
{
    public sealed record CreateChatCommand(string Name) : IRequest;
}
