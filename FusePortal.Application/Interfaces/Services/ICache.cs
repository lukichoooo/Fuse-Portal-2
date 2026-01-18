namespace FusePortal.Application.Interfaces.Services
{
    public interface ICache
    {
        Task<string?> GetValueAsync(string key);
        Task SetValueAsync(string key, string value);
    }
}
