using FusePortal.Application.Common;
using FusePortal.Application.Common.SeedWork;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.Interfaces.Services.File;
using FusePortal.Application.UseCases.Academic.Subjects.Exceptions;
using FusePortal.Domain.Entities.Academic.SubjectAggregate;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.CreateSyllabusesForSubjectFromFiles
{
    public class CreateSyllabusesForSubjectFromFilesCommandHandler : BaseCommandHandler<CreateSyllabusesForSubjectFromFilesCommand>
    {
        private readonly ISubjectRepo _repo;
        private readonly IIdentityProvider _identity;
        private readonly IFileProcessor _fileProcessor;

        public CreateSyllabusesForSubjectFromFilesCommandHandler(
                IIdentityProvider identity,
                ISubjectRepo repo,
                IFileProcessor fileProcessor,
                IUnitOfWork uow) : base(uow)
        {
            _repo = repo;
            _identity = identity;
            _fileProcessor = fileProcessor;
        }

        protected override async Task ExecuteAsync(CreateSyllabusesForSubjectFromFilesCommand command, CancellationToken ct)
        {
            var userId = _identity.GetCurrentUserId();
            var subject = await _repo.GetByIdAsync(command.SubjectId, userId)
                ?? throw new SubjectNotFoundException($"Subject with Id={command.SubjectId} not found");

            var fileDatas = await _fileProcessor.ProcessFilesAsync(command.FileUploads);

            foreach (var fileData in fileDatas)
                subject.AddSyllabus(fileData.Name, fileData.Text);
        }
    }
}
