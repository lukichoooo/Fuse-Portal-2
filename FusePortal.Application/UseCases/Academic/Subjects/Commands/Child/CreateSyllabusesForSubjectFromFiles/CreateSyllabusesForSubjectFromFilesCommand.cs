using FusePortal.Application.Interfaces.Services.File;
using MediatR;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.CreateSyllabusesForSubjectFromFiles
{
    public sealed record CreateSyllabusesForSubjectFromFilesCommand(
                List<FileUpload> FileUploads,
                Guid SubjectId) : IRequest;
}
