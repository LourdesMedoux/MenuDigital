using MenuDigital.Common.DTOs.Auth;

namespace MenuDigital.Services.Auth;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}
