using AssessmentEmployability.Application.DTOs.Auth;

namespace AssessmentEmployability.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
}
