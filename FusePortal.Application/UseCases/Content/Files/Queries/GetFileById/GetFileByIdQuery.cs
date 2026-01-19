using MediatR;

namespace FusePortal.Application.UseCases.Content.Files.Queries.GetFileById
{
    public sealed record GetFileByIdQuery(Guid FileId) : IRequest<FileDto>;
}
