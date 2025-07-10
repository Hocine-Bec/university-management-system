using Applications.DTOs.UserRole;
using Applications.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.ResultExtension;

namespace Presentation.Controllers;

[ApiController]
[Route("api/user-roles")]
[Authorize(Roles = "Admin")]
public class UserRolesController(IUserRoleService service) : ControllerBase
{
    [HttpPost("assign")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserRoleDto>> AssignRole(UserRoleDto request)
    {
        var response = await service.AssignRoleAsync(request);
        return response.HandleResult(nameof(GetByUserAndRole),
            new { userId = response.Value.UserId, roleId = response.Value.RoleId });
    }

    [HttpDelete("remove")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> RemoveRole(UserRoleDto request)
    {
        var response = await service.RemoveRoleAsync(request);
        return response.HandleResult();
    }

    [HttpPost("has-role")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> UserHasRole(UserRoleDto request)
    {
        var response = await service.UserHasRoleAsync(request);
        return response.HandleResult();
    }

    [HttpGet("has-role")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> UserHasRole(int userId, int roleType)
    {
        var response = await service.UserHasRoleAsync(userId, roleType);
        return response.HandleResult();
    }

    [HttpPost("by-user-and-role")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserRoleDto>> GetByUserAndRole(UserRoleDto request)
    {
        var response = await service.GetByUserAndRoleAsync(request);
        return response.HandleResult();
    }

    [HttpGet("by-user/{userId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<UserRoleDto>>> GetByUserId(int userId)
    {
        var response = await service.GetByUserIdAsync(userId);
        return response.HandleResult();
    }

    [HttpGet("by-role/{roleId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<UserRoleDto>>> GetByRoleId(int roleId)
    {
        var response = await service.GetByRoleIdAsync(roleId);
        return response.HandleResult();
    }

    [HttpGet("role-names/{userId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<RoleNameResponse>>> GetUserRoleNames(int userId)
    {
        var response = await service.GetUserRoleNamesAsync(userId);
        return response.HandleResult();
    }
}