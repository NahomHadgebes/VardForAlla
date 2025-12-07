using Microsoft.AspNetCore.Mvc;
using VardForAlla.Api.Dtos;
using VardForAlla.Application.Interfaces;

namespace VardForAlla.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    // GET: api/tag
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TagDto>>> GetAll()
    {
        var tags = await _tagService.GetAllAsync();

        var dtos = tags.Select(t => new TagDto
        {
            Id = t.Id,
            Name = t.Name
        }).ToList();

        return Ok(dtos);
    }

    // GET: api/tag/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TagDto>> GetById(int id)
    {
        var tag = await _tagService.GetByIdAsync(id);
        if (tag == null)
            return NotFound();

        var dto = new TagDto
        {
            Id = tag.Id,
            Name = tag.Name
        };

        return Ok(dto);
    }

    // POST: api/tag
    [HttpPost]
    public async Task<ActionResult<TagDto>> Create([FromBody] TagCreateDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var tag = await _tagService.CreateAsync(createDto.Name);

        var dto = new TagDto
        {
            Id = tag.Id,
            Name = tag.Name
        };

        return CreatedAtAction(nameof(GetById), new { id = tag.Id }, dto);
    }

    // PUT: api/tag/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TagUpdateDto updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var success = await _tagService.UpdateAsync(id, updateDto.Name);

        if (!success)
            return NotFound();

        return NoContent();
    }

    // DELETE: api/tag/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _tagService.DeleteAsync(id);

        if (!success)
            return NotFound();

        return NoContent();
    }
}

