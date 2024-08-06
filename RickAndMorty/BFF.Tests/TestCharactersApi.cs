using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using BFF.DTOs;
using BFF.Models;
using Moq;
using BFF.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Hosting;
using Xunit;

namespace BFF.Tests;

public class TestCharactersApi
{
    private readonly WebApplicationFactory<Program>? _builder;
    private Mock<ICharacter> mockCharacter;

    public TestCharactersApi()
    {
        mockCharacter = new Mock<ICharacter>();
        var application = new WebApplicationFactory<Program>();
        _builder = application.WithWebHostBuilder(builder => {
            builder.ConfigureServices(services => {
                services.AddSingleton(mockCharacter.Object);
            });
        });
    }

    [Fact]
    public async Task GetCharacters()
    {
        var characters = new Characters
        {
            Info = new Info
            {
                Count = 1,
            },
            Results = new List<Character>
            {
                new Character {
                    Name = "Rick"
                }
            }
        };
        using var application = new WebApplicationFactory<Program>();
        mockCharacter.Setup(c => c.GetCharacters(It.IsAny<CharacterFilterDTO>())).ReturnsAsync(characters);
        var client = _builder.CreateClient();
        
        var filter = new CharacterFilterDTO { };

        var response = await client.GetFromJsonAsync<Characters>("/Personagens/");
        var res = response.Results.FirstOrDefault();
        var exp = characters.Results.FirstOrDefault();
        Assert.Equal(exp.Name, res.Name);
        Assert.Equal(exp.Species, res.Species);
        Assert.Equal(exp.Status, res.Status);
        Assert.Equal(exp.Location, res.Location);
    }

    /*[Fact]
    public async Task TestGetCharactersOfAEpisode()
    {
        // Arrange
        var characters = new Character[] {
            new Character {
                Id = 1,
                Name = "Rick Sanches"
            },
            new Character {
                Id = 2,
                Name = "Morty"
            }
        };

        var mock = new Mock<ICharacter>();
        mock.Setup(x => x.GetCharactersByIds2("1,2")).ReturnsAsync(characters);
        await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateClient();

        // Act
        var response = await client.GetFromJsonAsync<ResponseCharactersOfEpisodeDTO>("/CharactersByEpisodeName");

        // Assert
        Assert.IsType<ResponseCharactersOfEpisodeDTO>(response);
        Assert.NotNull(response);
        Assert.NotNull(response.Characters);
        // Assert.True(response.Characters.Any());
        Assert.Equal(response.Characters.FirstOrDefault(), new Character
        {
            Id = 1,
            Name = "test"
        });
        // Assert.Equal(response.Characters.FirstOrDefault, new Character { Id = 1});
        // Assert.Equal("Hello World!", response);
    }*/

}