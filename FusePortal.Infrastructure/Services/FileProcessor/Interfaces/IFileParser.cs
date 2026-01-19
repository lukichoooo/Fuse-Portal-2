namespace FusePortal.Infrastructure.Services.FileProcessor.Interfaces
{
    public interface IFileParser
    {
        Task<string> ReadDocxAsync(Stream stream);
        Task<string> ReadTextAsync(Stream stream);
    }
}
