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
    private readonly IPasswordHasher _hasher;
    private readonly IMyLogger _logger;
    private readonly IValidationService _validator;

    public AuthenticationService(IUserRepository userRepository, IJwtTokenService jwtTokenService,
        IPasswordHasher hasher, IMyLogger logger, IValidationService validator)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _hasher = hasher;
        _logger = logger;
        _validator = validator;
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
                return Result<LoginResponse>.Failure("Invalid credentials", ErrorType.Unauthorized);

            if (!user.IsActive)
                return Result<LoginResponse>.Failure("Account is deactivated", ErrorType.Unauthorized);

            // Verify password
            if (!_hasher.VerifyPassword(request.Password, user.Password))
                return Result<LoginResponse>.Failure("Invalid credentials", ErrorType.Unauthorized);

            // Generate tokens
            var token = _jwtTokenService.GenerateToken(user);
            var expiresAt = _jwtTokenService.GetTokenExpiration();

            var response = new LoginResponse
            {
                Token = token,
                ExpiresAt = expiresAt,
            };

            return Result<LoginResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during login for user: {request.Username}",ex , new { request });
            return Result<LoginResponse>.Failure("An error occurred during login", ErrorType.InternalServerError);
        }
    }
}