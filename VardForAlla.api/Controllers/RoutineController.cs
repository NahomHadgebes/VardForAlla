using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VardForAlla.Api.DtoBuilder;
using VardForAlla.Api.Dtos;
using VardForAlla.Application.Interfaces;
using VardForAlla.Domain.Entities;

namespace VardForAlla.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoutineController : ControllerBase
{
    private readonly IRoutineService _routineService;
    private readonly RoutineDtoBuilder _dtoBuilder;

    public RoutineController(IRoutineService routineService, RoutineDtoBuilder dtoBuilder)
    {
        _routineService = routineService;
        _dtoBuilder = dtoBuilder;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoutineListDto>>> GetAll()
    {
        var routines = await _routineService.GetAllAsync();
        var dto = _dtoBuilder.BuildList(routines);
        return Ok(dto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RoutineDetailDto>> GetById(int id)
    {
        var routine = await _routineService.GetByIdAsync(id);

        var dto = _dtoBuilder.BuildDetail(routine);
        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<RoutineDetailDto>> Create([FromBody] RoutineCreateDto createDto)
    {
        var steps = createDto.Steps
            .Select(s => (s.Order, s.SimpleText, s.OriginalText, s.IconKey))
            .ToList();

        var routine = await _routineService.CreateRoutineAsync(
            createDto.Title,
            createDto.Category,
            createDto.SimpleDescription,
            createDto.OriginalDescription,
            steps);

        var dto = _dtoBuilder.BuildDetail(routine);
        return CreatedAtAction(nameof(GetById), new { id = routine.Id }, dto);

    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] RoutineUpdateDto updateDto)
    {
        var success = await _routineService.UpdateRoutineAsync(
            id,
            updateDto.Title,
            updateDto.Category,
            updateDto.SimpleDescription,
            updateDto.OriginalDescription);

        if (!success)
            throw new ArgumentNullException($"{id} not found");

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _routineService.DeleteRoutineAsync(id);

        if (!success)
            throw new ArgumentNullException();

        return NoContent();
    }


}

