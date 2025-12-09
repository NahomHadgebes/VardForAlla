using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VardForAlla.Api.DtoBuilder;
using VardForAlla.Api.Dtos;
using VardForAlla.Application.Interfaces;

namespace VardForAlla.Api.Controllers;

[ApiController]
[Route("api")]
public class RoutineStepController : ControllerBase
{
    private readonly IRoutineStepService _stepService;
    private readonly RoutineStepDtoBuilder _dtoBuilder;
    public RoutineStepController(IRoutineStepService stepService, RoutineStepDtoBuilder dtoBuilder)
    {
        _stepService = stepService;
        _dtoBuilder = dtoBuilder;
    }

    [HttpGet("routines/{routineId:int}/steps")]
    public async Task<ActionResult<IEnumerable<RoutineStepDto>>> GetForRoutine(int routineId)
    {
        var steps = await _stepService.GetByRoutineIdAsync(routineId);

       var dto = _dtoBuilder.BuildList(steps);

        return (dto);
    }

    [HttpPost("routines/{routineId:int}/steps")]
    public async Task<ActionResult<RoutineStepDto>> Create(int routineId,[FromBody] RoutineStepCreateDto dto)
    {

        var step = await _stepService.AddStepAsync(
            routineId,
            dto.Order,
            dto.SimpleText,
            dto.OriginalText,
            dto.IconKey);

        var result = _dtoBuilder.BuildItem(step);

        return CreatedAtAction(nameof(GetForRoutine), new { routineId }, result);
    }

    [HttpPut("steps/{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] RoutineStepUpdateDto dto)
    {
        var success = await _stepService.UpdateStepAsync(
            id,
            dto.Order,
            dto.SimpleText,
            dto.OriginalText,
            dto.IconKey);

        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("steps/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _stepService.DeleteStepAsync(id);

        if (!success)
            return NotFound();

        return NoContent();
    }
}

