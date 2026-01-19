namespace FusePortal.Domain.Entities.Content.FileEntityAggregate
{
    public interface IFileRepo
    {
        Task AddAsync(FileEntity fileE);
        Task<FileEntity?> GetById(Guid id, Guid UserId);
    }
}
