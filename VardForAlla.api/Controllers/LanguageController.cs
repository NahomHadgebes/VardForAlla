using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VardForAlla.Api.DtoBuilders;
using VardForAlla.Api.Dtos;
using VardForAlla.Application.Interfaces;

namespace VardForAlla.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LanguageController : ControllerBase
{
    private readonly ILanguageService _languageService;
    private readonly LanguageDtoBuilder _dtoBuilder;

    public LanguageController(ILanguageService languageService, LanguageDtoBuilder dtoBuilder)
    {
        _languageService = languageService;
        _dtoBuilder = dtoBuilder;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LanguageDto>>> GetAll()
    {
        var languages = await _languageService.GetAllAsync();

        var dto = _dtoBuilder.BuildList(languages);

        return (dto);
    }

    [HttpPost]
    public async Task<ActionResult<LanguageDto>> Create([FromBody] LanguageCreateDto createDto)
    {
        var languages = await _languageService.CreateAsync(createDto.Code, createDto.Name);

        var dto = _dtoBuilder.BuildItem(languages);

        return CreatedAtAction(nameof(GetAll), null, dto);
    }
}

