namespace FusePortal.Application.UseCases.Content.Files
{
    public interface IFileProcessor
    {
        Task<List<FileDto>> ProcessFilesAsync(List<FileUpload> files);
    }
}
