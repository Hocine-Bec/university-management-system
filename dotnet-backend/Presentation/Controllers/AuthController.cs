using Applications.DTOs.Auth;
using Applications.Interfaces.Auth;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.ResultExtension;

namespace Presentation.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthenticationService service) : ControllerBase
{
    [HttpPost("login")] 
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var response = await service.LoginAsync(request);
        return response.HandleResult();
    }
    
    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<LoginResponse>> RefreshToken(RefreshTokenRequest request)
    {
        var response = await service.RefreshTokenAsync(request);
        return response.HandleResult();
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Logout(RefreshTokenRequest request)
    {
        var response = await service.LogoutAsync(request.RefreshToken);
        return response.HandleResult();
    }
}