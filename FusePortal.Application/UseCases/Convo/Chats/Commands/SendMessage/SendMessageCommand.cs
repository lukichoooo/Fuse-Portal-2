using FusePortal.Application.Interfaces.Services.File;
using MediatR;

namespace FusePortal.Application.UseCases.Convo.Chats.Commands.SendMessage
{
    public sealed record SendMessageCommand(
            Guid ChatId,
            string MessageText,
            List<FileUpload> FileUploads,
            bool Streaming) : IRequest;
}
