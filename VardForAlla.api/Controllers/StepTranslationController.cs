using Microsoft.AspNetCore.Mvc;
using VardForAlla.Api.Dtos;
using VardForAlla.Application.Interfaces;

namespace VardForAlla.Api.Controllers;

[ApiController]
[Route("api")]
public class StepTranslationController : ControllerBase
{
    private readonly IStepTranslationService _translationService;
    private readonly ILanguageService _languageService;

    public StepTranslationController(
        IStepTranslationService translationService,
        ILanguageService languageService)
    {
        _translationService = translationService;
        _languageService = languageService;
    }

    // GET: api/steps/{stepId}/translations
    [HttpGet("steps/{stepId:int}/translations")]
    public async Task<ActionResult<IEnumerable<StepTranslationDto>>> GetForStep(int stepId)
    {
        var translations = await _translationService.GetForStepAsync(stepId);

        var dtos = translations.Select(t => new StepTranslationDto
        {
            Id = t.Id,
            StepId = t.RoutineStepId,
            LanguageCode = t.Language.Code,
            Text = t.Text
        }).ToList();

        return Ok(dtos);
    }

    // POST: api/steps/{stepId}/translations
    [HttpPost("steps/{stepId:int}/translations")]
    public async Task<ActionResult<StepTranslationDto>> Create(int stepId, [FromBody] StepTranslationCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var translation = await _translationService.AddTranslationAsync(stepId, dto.LanguageCode, dto.Text);

        var result = new StepTranslationDto
        {
            Id = translation.Id,
            StepId = translation.RoutineStepId,
            LanguageCode = dto.LanguageCode,
            Text = translation.Text
        };

        return CreatedAtAction(nameof(GetForStep), new { stepId = stepId }, result);
    }

    // PUT: api/translations/{id}
    [HttpPut("translations/{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] StepTranslationUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var success = await _translationService.UpdateTranslationAsync(id, dto.Text);

        if (!success)
            return NotFound();

        return NoContent();
    }

    // DELETE: api/translations/{id}
    [HttpDelete("translations/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _translationService.DeleteTranslationAsync(id);

        // Just nu antar vi att delete alltid "går", vill du kan du bygga ut till bool-check.
        return NoContent();
    }
}

