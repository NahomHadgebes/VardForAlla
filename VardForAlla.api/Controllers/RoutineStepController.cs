using Microsoft.AspNetCore.Mvc;
using VardForAlla.Api.Dtos;
using VardForAlla.Application.Interfaces;

namespace VardForAlla.Api.Controllers;

[ApiController]
[Route("api")]
public class RoutineStepController : ControllerBase
{
    private readonly IRoutineStepService _stepService;
    public RoutineStepController(IRoutineStepService stepService)
    {
        _stepService = stepService;
    }

    // GET: api/routines/{routineId}/steps
    [HttpGet("routines/{routineId:int}/steps")]
    public async Task<ActionResult<IEnumerable<RoutineStepDto>>> GetForRoutine(int routineId)
    {
        var steps = await _stepService.GetByRoutineIdAsync(routineId);

        var dtos = steps
            .OrderBy(s => s.Order)
            .Select(s => new RoutineStepDto
            {
                Order = s.Order,
                SimpleText = s.SimpleText,
                OriginalText = s.OriginalText,
                IconKey = s.IconKey
            }).ToList();

        return Ok(dtos);
    }

    // POST: api/routines/{routineId}/steps
    [HttpPost("routines/{routineId:int}/steps")]
    public async Task<ActionResult<RoutineStepDto>> Create(
        int routineId,
        [FromBody] RoutineStepCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var step = await _stepService.AddStepAsync(
            routineId,
            dto.Order,
            dto.SimpleText,
            dto.OriginalText,
            dto.IconKey);

        var result = new RoutineStepDto
        {
            Order = step.Order,
            SimpleText = step.SimpleText,
            OriginalText = step.OriginalText,
            IconKey = step.IconKey
        };

        return CreatedAtAction(nameof(GetForRoutine), new { routineId = routineId }, result);
    }

    // PUT: api/steps/{id}
    [HttpPut("steps/{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] RoutineStepUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

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

    // DELETE: api/steps/{id}
    [HttpDelete("steps/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _stepService.DeleteStepAsync(id);

        if (!success)
            return NotFound();

        return NoContent();
    }
}

