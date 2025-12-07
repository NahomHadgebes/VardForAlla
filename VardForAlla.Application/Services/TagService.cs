using Microsoft.Extensions.Logging;
using VardForAlla.Application.Interfaces;
using VardForAlla.Domain.Entities;

namespace VardForAlla.Application.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly ILogger<TagService> _logger;

    public TagService(ITagRepository tagRepository, ILogger<TagService> logger)
    {
        _tagRepository = tagRepository;
        _logger = logger;
    }

    public async Task<List<Tag>> GetAllAsync()
    {
        _logger.LogInformation("Hämtar alla taggar.");
        return await _tagRepository.GetAllAsync();
    }

    public async Task<Tag?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Hämtar tagg med id {Id}.", id);
        return await _tagRepository.GetByIdAsync(id);
    }

    public async Task<Tag> CreateAsync(string name)
    {
        _logger.LogInformation("Försöker skapa ny tagg med namn {Name}.", name);

        var tag = new Tag
        {
            Name = name
        };

        await _tagRepository.AddAsync(tag);

        _logger.LogInformation("Tagg skapad med id {Id}.", tag.Id);

        return tag;
    }

    public async Task<bool> UpdateAsync(int id, string name)
    {
        _logger.LogInformation("Försöker uppdatera tagg med id {Id}.", id);

        var tag = await _tagRepository.GetByIdAsync(id);
        if (tag == null)
        {
            _logger.LogWarning("Ingen tagg hittades med id {Id}.", id);
            return false;
        }

        tag.Name = name;
        await _tagRepository.UpdateAsync(tag);

        _logger.LogInformation("Tagg med id {Id} uppdaterad.", id);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Försöker ta bort tagg med id {Id}.", id);

        var tag = await _tagRepository.GetByIdAsync(id);
        if (tag == null)
        {
            _logger.LogWarning("Ingen tagg hittades med id {Id}.", id);
            return false;
        }

        await _tagRepository.DeleteAsync(id);

        _logger.LogInformation("Tagg med id {Id} borttagen.", id);

        return true;
    }
}

