namespace FusePortal.Domain.Entities.FileEntityAggregate
{
    public interface IFileRepo
    {
        Task AddFilesAsync(List<FileEntity> files);

        ValueTask<FileEntity?> GetFileByIdAsync(int fileId, int userId);
        Task<FileEntity> RemoveFileByIdAsync(int fileId, int userId);
    }
}
