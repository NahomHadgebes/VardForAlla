using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VardForAlla.Api.DtoBuilder;
using VardForAlla.Api.Dtos;
using VardForAlla.Application.Interfaces;

namespace VardForAlla.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoutineController : ControllerBase
{
    private readonly IRoutineService _routineService;
    private readonly RoutineDtoBuilder _dtoBuilder;

    public RoutineController(IRoutineService routineService, RoutineDtoBuilder dtoBuilder)
    {
        _routineService = routineService;
        _dtoBuilder = dtoBuilder;
    }

    private int GetCurrentUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    private bool IsAdmin()
    {
        return User.IsInRole("Admin");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoutineListDto>>> GetAll()
    {
        var userId = IsAdmin() ? (int?)null : GetCurrentUserId();
        var routines = await _routineService.GetAllAsync(userId, includeTemplates: true);
        var dto = _dtoBuilder.BuildList(routines);
        return Ok(dto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RoutineDetailDto>> GetById(int id)
    {
        var userId = IsAdmin() ? (int?)null : GetCurrentUserId();
        var routine = await _routineService.GetByIdAsync(id, userId);

        if (routine == null)
        {
            return NotFound();
        }

        var dto = _dtoBuilder.BuildDetail(routine);
        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<RoutineDetailDto>> Create([FromBody] RoutineCreateDto createDto)
    {
        var steps = createDto.Steps
            .Select(s => (s.Order, s.SimpleText, s.OriginalText, s.IconKey))
            .ToList();

        var userId = IsAdmin() ? (int?)null : GetCurrentUserId();
        var isTemplate = IsAdmin();

        var routine = await _routineService.CreateRoutineAsync(
            createDto.Title,
            createDto.Category,
            createDto.SimpleDescription,
            createDto.OriginalDescription,
            steps,
            userId,
            isTemplate);

        var dto = _dtoBuilder.BuildDetail(routine);
        return CreatedAtAction(nameof(GetById), new { id = routine.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] RoutineUpdateDto updateDto)
    {
        var userId = IsAdmin() ? (int?)null : GetCurrentUserId();

        var success = await _routineService.UpdateRoutineAsync(
            id,
            updateDto.Title,
            updateDto.Category,
            updateDto.SimpleDescription,
            updateDto.OriginalDescription,
            userId);

        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = IsAdmin() ? (int?)null : GetCurrentUserId();
        var success = await _routineService.DeleteRoutineAsync(id, userId);

        if (!success)
            return NotFound();

        return NoContent();
    }
}

