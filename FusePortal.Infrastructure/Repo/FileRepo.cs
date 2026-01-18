using FusePortal.Domain.Entities.Content.FileEntityAggregate;

namespace FusePortal.Infrastructure.Repo
{
    public class FileRepo : IFileRepo
    {
        public Task AddFilesAsync(List<FileEntity> files)
        {
            throw new NotImplementedException();
        }

        public ValueTask<FileEntity?> GetFileByIdAsync(Guid fileId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<FileEntity> RemoveFileByIdAsync(Guid fileId, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
