using MediatR;

namespace FusePortal.Application.Chats.Commands.CreateChat
{
    public sealed record CreateChatCommand(string Name) : IRequest;
}
