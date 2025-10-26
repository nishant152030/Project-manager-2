using ProjectManagementAPI.DTOs;

namespace ProjectManagementAPI.Services;

public interface IAuthService
{
    Task<string?> RegisterAsync(RegisterDto dto);
    Task<string?> LoginAsync(LoginDto dto);
}
