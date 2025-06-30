using Applications.DTOs.Course;
using Applications.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.ResultExtension;

namespace Presentation.Controllers;

[ApiController]
[Route("api/courses")]
public class CoursesController(ICourseService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<CourseResponse>>> GetList()
    {
        var response = await service.GetListAsync();
        return !response.IsSuccess ? response.HandleResult() : Ok(response.Value);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CourseResponse>> GetById(int id)
    {
        var response = await service.GetByIdAsync(id);
        return !response.IsSuccess ? response.HandleResult() : Ok(response.Value);
    }

    [HttpGet("by-code/{code}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CourseResponse>> GetByCourseCode(string code)
    {
        var response = await service.GetByCodeAsync(code);
        return !response.IsSuccess ? response.HandleResult() : Ok(response.Value);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CourseResponse>> Create(CourseRequest request)
    {
        var response = await service.AddAsync(request);

        if (!response.IsSuccess)
            return response.HandleResult();

        return CreatedAtAction(nameof(GetById), new { id = response.Value.Id }, response.Value);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update(int id, CourseRequest request)
    {
        var response = await service.UpdateAsync(id, request);
        return !response.IsSuccess ? response.HandleResult() : NoContent();
    }

    [HttpPatch("{id}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeactivateCourse(int id)
    {
        var response = await service.DeactivateCourseAsync(id);
        return !response.IsSuccess ? response.HandleResult() : NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        var response = await service.DeleteAsync(id);
        return !response.IsSuccess ? response.HandleResult() : NoContent();
    }
}