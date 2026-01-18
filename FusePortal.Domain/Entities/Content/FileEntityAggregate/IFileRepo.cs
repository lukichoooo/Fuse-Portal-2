namespace FusePortal.Domain.Entities.Content.FileEntityAggregate
{
    public interface IFileRepo
    {
        Task AddFilesAsync(List<FileEntity> files);

        ValueTask<FileEntity?> GetFileByIdAsync(Guid fileId, Guid userId);
        Task<FileEntity> RemoveFileByIdAsync(Guid fileId, Guid userId);
    }
}
