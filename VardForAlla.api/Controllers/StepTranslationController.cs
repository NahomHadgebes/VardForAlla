using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VardForAlla.Api.DtoBuilder;
using VardForAlla.Api.Dtos;
using VardForAlla.Application.Interfaces;

namespace VardForAlla.Api.Controllers;

[ApiController]
[Route("api")]
public class StepTranslationController : ControllerBase
{
    private readonly IStepTranslationService _translationService;
    private readonly ILanguageService _languageService;
    private readonly StepTranslationDtoBuilder _dtoBuilder;

    public StepTranslationController(IStepTranslationService translationService, ILanguageService languageService, StepTranslationDtoBuilder dtoBuilder)
    {
        _translationService = translationService;
        _languageService = languageService;
        _dtoBuilder = dtoBuilder;
    }

    [HttpGet("steps/{stepId:int}/translations")]
    public async Task<ActionResult<IEnumerable<StepTranslationDto>>> GetForStep(int stepId)
    {
        var translations = await _translationService.GetForStepAsync(stepId);

        var dtos = _dtoBuilder.BuildList(translations);

        return Ok(dtos);
    }

    [HttpPost("steps/{stepId:int}/translations")]
    public async Task<ActionResult<StepTranslationDto>> Create(int stepId, [FromBody] StepTranslationCreateDto dto)
    {
        var translation = await _translationService.AddTranslationAsync(stepId, dto.LanguageCode, dto.TranslatedText);

        var result = _dtoBuilder.BuildItem(translation);

        return CreatedAtAction(nameof(GetForStep), new { stepId }, result);
    }

    [HttpPut("translations/{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] StepTranslationUpdateDto dto)
    {

        var success = await _translationService.UpdateTranslationAsync(id, dto.Text);

        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("translations/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _translationService.DeleteTranslationAsync(id);

        return NoContent();
    }
}

