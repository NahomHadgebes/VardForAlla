using Microsoft.AspNetCore.Mvc;
using VardForAlla.Api.Dtos;
using VardForAlla.Application.Interfaces;

namespace VardForAlla.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LanguageController : ControllerBase
{
    private readonly ILanguageService _languageService;

    public LanguageController(ILanguageService languageService)
    {
        _languageService = languageService;
    }

    // GET: api/language
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LanguageDto>>> GetAll()
    {
        var languages = await _languageService.GetAllAsync();

        var result = languages.Select(l => new LanguageDto
        {
            Id = l.Id,
            Code = l.Code,
            Name = l.Name
        }).ToList();

        return Ok(result);
    }

    // POST: api/language
    [HttpPost]
    public async Task<ActionResult<LanguageDto>> Create([FromBody] LanguageCreateDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var language = await _languageService.CreateAsync(createDto.Code, createDto.Name);

        var dto = new LanguageDto
        {
            Id = language.Id,
            Code = language.Code,
            Name = language.Name
        };

        return CreatedAtAction(nameof(GetAll), null, dto);
    }
}

