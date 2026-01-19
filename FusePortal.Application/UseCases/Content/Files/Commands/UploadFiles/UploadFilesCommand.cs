using MediatR;

namespace FusePortal.Application.UseCases.Content.Files.Commands.UploadFiles
{
    public sealed record UploadFilesCommand(List<FileUpload> FileUploads)
        : IRequest<List<Guid>>;
}
