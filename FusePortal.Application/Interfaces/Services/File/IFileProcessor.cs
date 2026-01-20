using FusePortal.Domain.Common.Objects;

namespace FusePortal.Application.Interfaces.Services.File
{
    public interface IFileProcessor
    {
        Task<List<FileData>> ProcessFilesAsync(List<FileUpload> files);
    }
}
