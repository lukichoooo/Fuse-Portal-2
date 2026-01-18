namespace FusePortal.Infrastructure.Services.LLM.Interfaces
{
    public interface IChatMetadataService
    {
        Task<string?> GetLastResponseIdAsync(Guid chatId);
        Task SetLastResponseIdAsync(Guid chatId, string responseId);
    }
}
