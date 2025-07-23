using Applications.DTOs.Auth;
using Applications.Shared;

namespace Applications.Interfaces.Auth;

public interface IAuthenticationService
{
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
}
