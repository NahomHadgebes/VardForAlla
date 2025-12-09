using Microsoft.AspNetCore.Mvc;
using VardForAlla.Api.Dtos;
using VardForAlla.Application.Interfaces;
using VardForAlla.Api.DtoBuilder;


namespace VardForAlla.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;
    private readonly TagDtoBuilder _dtoBuilder;

    public TagController(ITagService tagService, TagDtoBuilder dtoBuilder)
    {
        _tagService = tagService;
        _dtoBuilder = dtoBuilder;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TagDto>>> GetAll()
    {
        var tags = await _tagService.GetAllAsync();

        var dto = _dtoBuilder.BuildList(tags);

        return Ok(dto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TagDto>> GetById(int id)
    {
        var tag = await _tagService.GetByIdAsync(id);
        if (tag == null)
            return NotFound();

        var dto = _dtoBuilder.BuildItem(tag);

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<TagDto>> Create([FromBody] TagCreateDto createDto)
    {
        var tag = await _tagService.CreateAsync(createDto.Name);

        var dto = _dtoBuilder.BuildItem(tag);

        return CreatedAtAction(nameof(GetById), new { id = tag.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TagUpdateDto updateDto)
    {

        var success = await _tagService.UpdateAsync(id, updateDto.Name);

        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _tagService.DeleteAsync(id);

        if (!success)
            return NotFound();

        return NoContent();
    }
}

