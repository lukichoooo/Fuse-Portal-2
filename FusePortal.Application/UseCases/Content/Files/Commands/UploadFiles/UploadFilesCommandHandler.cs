using FusePortal.Application.Common;
using FusePortal.Application.Common.SeedWork;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Domain.Entities.Content.FileEntityAggregate;

namespace FusePortal.Application.UseCases.Content.Files.Commands.UploadFiles
{
    public class UploadFilesCommandHandler
        : BaseCommandHandler<UploadFilesCommand, List<Guid>>
    {
        private readonly IFileRepo _repo;
        private readonly IIdentityProvider _identity;
        private readonly IFileProcessor _fileService;

        public UploadFilesCommandHandler(
                IFileRepo repo,
                IFileProcessor processor,
                IIdentityProvider identity,
                IUnitOfWork uow) : base(uow)
        {
            _repo = repo;
            _fileService = processor;
            _identity = identity;
        }


        protected override async Task<List<Guid>> ExecuteAsync(UploadFilesCommand command, CancellationToken ct)
        {
            var userId = _identity.GetCurrentUserId();

            var fileDtos = await _fileService.ProcessFilesAsync(command.FileUploads);

            var files = fileDtos.ConvertAll(f => new FileEntity(f.Name, f.Text, userId));

            foreach (var f in files)
                await _repo.AddAsync(f);

            return files.ConvertAll(f => f.Id);
        }
    }
}
