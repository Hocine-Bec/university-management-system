using Applications.DTOs.Auth;
using Applications.Interfaces.Auth;
using Applications.Interfaces.Logging;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using Applications.Shared;
using Domain.Enums;

namespace Applications.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _hasher;
    private readonly IMyLogger _logger;
    private readonly IValidationService _validator;

    public AuthenticationService(IUserRepository userRepository, IJwtTokenService jwtTokenService,
        IPasswordHasher hasher, IMyLogger logger, IValidationService validator, IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _hasher = hasher;
        _logger = logger;
        _validator = validator;
        _refreshTokenRepository = refreshTokenRepository;
    }
    
    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsSuccess)
            return Result<LoginResponse>.Failure(validationResult.Error, validationResult.ErrorType);
        
        try
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null)
                return Result<LoginResponse>.Failure("Invalid credentials", ErrorType.NotFound);

            if (!user.IsActive)
                return Result<LoginResponse>.Failure("Account is deactivated", ErrorType.Unauthorized);

            // Verify password
            if (!_hasher.VerifyPassword(request.Password, user.Password))
                return Result<LoginResponse>.Failure("Invalid credentials", ErrorType.Unauthorized);

            // Generate tokens
            var accessToken = _jwtTokenService.GenerateToken(user);
            var refreshToken = _jwtTokenService.GenerateRefreshToken(user.Id);
            
            // Save refresh token
            await _refreshTokenRepository.AddAsync(refreshToken);
            
            var response = new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                AccessTokenExpires = _jwtTokenService.GetTokenExpiration(),
                RefreshTokenExpires = refreshToken.ExpiresAt
            };

            return Result<LoginResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during login for user: {request.Username}",ex , new { request });
            return Result<LoginResponse>.Failure("An error occurred during login", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        if(!string.IsNullOrEmpty(request.RefreshToken))
            return Result<LoginResponse>.Failure("Invalid refresh token", ErrorType.BadRequest);
        try
        {
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
            if(storedToken == null)
                return Result<LoginResponse>.Failure("Invalid refresh token", ErrorType.NotFound);
            
            if(!_jwtTokenService.ValidateRefreshToken(storedToken))
                return Result<LoginResponse>.Failure("Refresh token expired or revoked", ErrorType.Unauthorized);
            
            // Revoke old token (token rotation)
            await _refreshTokenRepository.RevokeTokenAsync(storedToken.Token, "Used for refresh");
            
            // Generate new tokens
            var newAccessToken = _jwtTokenService.GenerateToken(storedToken.User);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken(storedToken.UserId);

            await _refreshTokenRepository.AddAsync(newRefreshToken);

            var response = new LoginResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token,
                AccessTokenExpires = _jwtTokenService.GetTokenExpiration(),
                RefreshTokenExpires = newRefreshToken.ExpiresAt
            };

            return Result<LoginResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error during token refresh", ex, new { request });
            return Result<LoginResponse>.Failure("An error occurred during token refresh", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> LogoutAsync(string refreshToken)
    {
        try
        {
            await _refreshTokenRepository.RevokeTokenAsync(refreshToken, "User logout");
            return Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error during logout", ex);
            return Result.Failure("An error occurred during logout", ErrorType.InternalServerError);
        }
    }
}