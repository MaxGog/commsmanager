using CommsManager.Application.DTOs.Auth;

namespace CommsManager.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken = default);
    Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default);
    Task PromoteToCreatorAsync(Guid userId, CancellationToken cancellationToken = default);
}
