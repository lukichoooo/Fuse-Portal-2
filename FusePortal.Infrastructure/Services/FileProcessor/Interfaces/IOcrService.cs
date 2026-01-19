namespace FusePortal.Infrastructure.Services.FileProcessor.Interfaces
{
    public interface IOcrService
    {
        Task<string> ProcessAsync(Stream fileStream);
        Task<string> FallbackOcrAsync(Stream fileStream);
    }
}
