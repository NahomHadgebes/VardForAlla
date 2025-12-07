using Microsoft.AspNetCore.Mvc;
using VardForAlla.Api.Dtos;
using VardForAlla.Application.Interfaces;
using VardForAlla.Domain.Entities;

namespace VardForAlla.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoutineController : ControllerBase
{
    private readonly IRoutineService _routineService;

    public RoutineController(IRoutineService routineService)
    {
        _routineService = routineService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoutineListDto>>> GetAll()
    {
        var routines = await _routineService.GetAllAsync();

        var result = routines.Select(r => new RoutineListDto
        {
            Id = r.Id,
            Title = r.Title,
            Category = r.Category,
            IsActive = r.IsActive
        }).ToList();

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RoutineDetailDto>> GetById(int id)
    {
        var routine = await _routineService.GetByIdAsync(id);

        if (routine == null)
            return NotFound();

        var dto = new RoutineDetailDto
        {
            Id = routine.Id,
            Title = routine.Title,
            Category = routine.Category,
            SimpleDescription = routine.SimpleDescription,
            OriginalDescription = routine.OriginalDescription,
            Steps = routine.Steps
                .OrderBy(s => s.Order)
                .Select(s => new RoutineStepDto
                {
                    Order = s.Order,
                    SimpleText = s.SimpleText,
                    OriginalText = s.OriginalText,
                    IconKey = s.IconKey
                }).ToList()
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<RoutineDetailDto>> Create([FromBody] RoutineCreateDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var steps = createDto.Steps
            .Select(s => (s.Order, s.SimpleText, s.OriginalText, s.IconKey))
            .ToList();

        var routine = await _routineService.CreateRoutineAsync(
            createDto.Title,
            createDto.Category,
            createDto.SimpleDescription,
            createDto.OriginalDescription,
            steps);

        var dto = new RoutineDetailDto
        {
            Id = routine.Id,
            Title = routine.Title,
            Category = routine.Category,
            SimpleDescription = routine.SimpleDescription,
            OriginalDescription = routine.OriginalDescription,
            Steps = routine.Steps
                .OrderBy(s => s.Order)
                .Select(s => new RoutineStepDto
                {
                    Order = s.Order,
                    SimpleText = s.SimpleText,
                    OriginalText = s.OriginalText,
                    IconKey = s.IconKey
                }).ToList()
        };

        return CreatedAtAction(nameof(GetById), new { id = routine.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] RoutineUpdateDto updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var success = await _routineService.UpdateRoutineAsync(
            id,
            updateDto.Title,
            updateDto.Category,
            updateDto.SimpleDescription,
            updateDto.OriginalDescription);

        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _routineService.DeleteRoutineAsync(id);

        if (!success)
            return NotFound();

        return NoContent();
    }


}

