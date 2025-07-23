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
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var response = await service.LoginAsync(request);
        return response.HandleResult();
    }
}