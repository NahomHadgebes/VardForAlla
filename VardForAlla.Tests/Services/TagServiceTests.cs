using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VardForAlla.Application.Interfaces;
using VardForAlla.Application.Services;
using VardForAlla.Domain.Entities;
using Xunit;

namespace VardForAlla.Tests.Services;

public class TagServiceTests
{
    private readonly Mock<ITagRepository> _tagRepoMock;
    private readonly TagService _sut;

    public TagServiceTests()
    {
        _tagRepoMock = new Mock<ITagRepository>();
        var logger = NullLogger<TagService>.Instance;

        _sut = new TagService(_tagRepoMock.Object, logger);
    }

    // ======================================================
    // GET ALL
    // ======================================================

    [Fact]
    public async Task GetAllAsync_Nar_Inga_Tags_Finns_Ska_Returnera_Tom_Lista()
    {
        // ARRANGE
        _tagRepoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Tag>());

        // ACT
        var result = await _sut.GetAllAsync();

        // ASSERT
        Assert.NotNull(result);
        Assert.Empty(result);

        _tagRepoMock.Verify(r => r.GetAllAsync(), Times.Once);
        _tagRepoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetAllAsync_Nar_Tags_Finns_Ska_Returnera_Dom()
    {
        // ARRANGE
        var tags = new List<Tag>
        {
            new() { Id = 1, Name = "Hygien" },
            new() { Id = 2, Name = "Mat" }
        };

        _tagRepoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(tags);

        // ACT
        var result = await _sut.GetAllAsync();

        // ASSERT
        Assert.Equal(2, result.Count);
        Assert.Contains(result, t => t.Id == 1 && t.Name == "Hygien");
        Assert.Contains(result, t => t.Id == 2 && t.Name == "Mat");

        _tagRepoMock.Verify(r => r.GetAllAsync(), Times.Once);
        _tagRepoMock.VerifyNoOtherCalls();
    }

    // ======================================================
    // CREATE
    // ======================================================

    [Fact]
    public async Task CreateAsync_Ska_Skapa_Ny_Tag_Och_Anropa_AddAsync()
    {
        // ARRANGE
        _tagRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Tag>()))
            .Returns(Task.CompletedTask);

        // ACT
        var tag = await _sut.CreateAsync("Hygien");

        // ASSERT
        Assert.NotNull(tag);
        Assert.Equal("Hygien", tag.Name);

        _tagRepoMock.Verify(r => r.AddAsync(It.Is<Tag>(t => t.Name == "Hygien")), Times.Once);
        _tagRepoMock.VerifyNoOtherCalls();
    }

    // ======================================================
    // DELETE
    // ======================================================

    [Fact]
    public async Task DeleteAsync_Nar_Tag_Finns_Ska_Ta_Bort_Och_Returnera_True()
    {
        // ARRANGE
        var tag = new Tag { Id = 5, Name = "Hygien" };

        _tagRepoMock
            .Setup(r => r.GetByIdAsync(5))
            .ReturnsAsync(tag);

        // OBS: Justera beroende på din repo-signatur:
        // om du har Task DeleteAsync(Tag tag) -> använd DeleteAsync(tag)
        // om du har Task DeleteAsync(int id)  -> använd DeleteAsync(5)
        _tagRepoMock
            .Setup(r => r.DeleteAsync(5))          // ändra till DeleteAsync(tag) vid behov
            .Returns(Task.CompletedTask);

        // ACT
        var result = await _sut.DeleteAsync(5);

        // ASSERT
        Assert.True(result);

        _tagRepoMock.Verify(r => r.GetByIdAsync(5), Times.Once);
        _tagRepoMock.Verify(r => r.DeleteAsync(5), Times.Once); // eller DeleteAsync(tag)
        _tagRepoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task DeleteAsync_Ska_Returnera_False_Nar_Tag_Ej_Finns_Och_Inte_Anropa_Delete()
    {
        // ARRANGE
        _tagRepoMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Tag?)null);

        // ACT
        var result = await _sut.DeleteAsync(999);

        // ASSERT
        Assert.False(result);

        _tagRepoMock.Verify(r => r.GetByIdAsync(999), Times.Once);
        _tagRepoMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never); // eller It.IsAny<Tag>()
        _tagRepoMock.VerifyNoOtherCalls();
    }
}


