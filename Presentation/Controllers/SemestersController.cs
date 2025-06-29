using Applications.DTOs.Semester;
using Applications.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.ResultExtension;

namespace Presentation.Controllers;

[ApiController]
[Route("api/semesters")]
public class SemestersController(ISemesterService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<SemesterResponse>>> GetList()
    {
        var response = await service.GetListAsync();
        return !response.IsSuccess ? response.HandleResult() : Ok(response.Value);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SemesterResponse>> GetById(int id)
    {
        var response = await service.GetByIdAsync(id);
        return !response.IsSuccess ? response.HandleResult() : Ok(response.Value);
    }

    [HttpGet("by-term-code/{termCode}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SemesterResponse>> GetByTermCode(string termCode)
    {
        var response = await service.GetByTermCodeAsync(termCode);
        return !response.IsSuccess ? response.HandleResult() : Ok(response.Value);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SemesterResponse>> Create(SemesterRequest request)
    {
        var response = await service.AddAsync(request);

        if (!response.IsSuccess)
            return response.HandleResult();

        return CreatedAtAction(nameof(GetById), new { id = response.Value.Id }, response.Value);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update(int id, SemesterRequest request)
    {
        var response = await service.UpdateAsync(id, request);
        return !response.IsSuccess ? response.HandleResult() : NoContent();
    }

    [HttpPatch("{id:int}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Deactivate(int id)
    {
        var response = await service.DeactivateSemesterAsync(id);
        return !response.IsSuccess ? response.HandleResult() : NoContent();
    }

    [HttpDelete("{id:int}")]
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