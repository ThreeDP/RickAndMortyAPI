using BFF.DTOs;
using BFF.Models;
using BFF.Repositories;
using BFF.Tests.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http.Json;

namespace BFF.Tests;

public class EpisodeEndPointTest
{
    private readonly EpisodeTestRepository _repoEpisode;
    private readonly CharactersTestRepository _repoCharacter;
    private readonly WebApplicationFactory<Program>? _builder;
    private Mock<IEpisodeGateway> mockEpisode;
    private Mock<ICharacterGateway> mockCharacter;

    public EpisodeEndPointTest()
    {
        mockEpisode = new Mock<IEpisodeGateway>();
        mockCharacter = new Mock<ICharacterGateway>();
        var application = new WebApplicationFactory<Program>();
        _builder = application.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(mockEpisode.Object);
                services.AddSingleton(mockCharacter.Object);
            });
        });
        _repoEpisode = new EpisodeTestRepository();
        _repoCharacter = new CharactersTestRepository();
    }

    [Fact]
    public async Task TestGetCharactersByEpisodeWithoutFilter()
    {
        // Given
        mockEpisode.Setup(ep => ep.GetEpisodesByName(It.IsAny<EpisodeFilterDTO>())).ReturnsAsync(_repoEpisode.Episodes);
        mockCharacter.Setup(c => c.GetCharactersByIds(It.IsAny<string>())).ReturnsAsync(_repoCharacter.Characters.Results.ToList());
        var client = _builder!.CreateClient();

        // When
        var response = await client.GetFromJsonAsync<ResponseCharactersOfEpisodeDTO>("/CharactersByEpisodeName/");

        // Then
        Assert.NotNull(response);
        Assert.IsType<ResponseCharactersOfEpisodeDTO>(response);
        Assert.Equal(response.Characters, _repoCharacter.Characters.Results);
    }
}

