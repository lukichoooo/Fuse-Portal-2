using FusePortal.Application.Auth;

namespace FusePortal.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest userRegister);
        Task<AuthResponse> LoginAsync(LoginRequest userLogin);
    }
}
