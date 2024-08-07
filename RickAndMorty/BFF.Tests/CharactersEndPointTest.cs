using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using BFF.DTOs;
using BFF.Models;
using Moq;
using BFF.Repositories;
using Microsoft.Extensions.DependencyInjection;
using BFF.Tests.Repositories;
using System.Net;
using System.Text.Json;
using System.Diagnostics.CodeAnalysis;
using Refit;

namespace BFF.Tests;

public class CharacterEndPointTest
{
    // Setup
    private readonly CharactersTestRepository _repo;
    private readonly WebApplicationFactory<Program>? _builder;
    private Mock<ICharacterGateway> mockCharacter;
    private JsonSerializerOptions _jsonOptions;
public CharacterEndPointTest()
    {
        mockCharacter = new Mock<ICharacterGateway>();
        var application = new WebApplicationFactory<Program>();
        _builder = application.WithWebHostBuilder(builder => {
            builder.ConfigureServices(services => {
                services.AddSingleton(mockCharacter.Object);
            });
        });
        _repo = new CharactersTestRepository();
        _jsonOptions = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        };
    }

    [Fact]
    public async Task GetCharacters()
    {
        // Given
        mockCharacter.Setup(c => c.GetCharacters(It.IsAny<CharacterFilterDTO>())).ReturnsAsync(_repo.Characters);
        var client = _builder!.CreateClient();

        // When
        var response = await client.GetAsync("/Personagens/");

        // Then
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        var body = JsonSerializer.Deserialize<Characters>(responseBody, _jsonOptions);
        Assert.NotNull(body);
        Assert.Equal(_repo.Characters, body);
    }

    [Fact]
    public async Task GetCharactersWithFilter()
    {
        // Given
        Characters expected = _repo.Characters; 
        expected.Results = _repo
            .Characters
            .Results
            .Where(n => n.Name
                .ToLower()
                .StartsWith("ri"));
        mockCharacter.Setup(c => c.GetCharacters(It.IsAny<CharacterFilterDTO>())).ReturnsAsync(_repo.Characters);
        var client = _builder!.CreateClient();

        // When
        var response = await client.GetAsync("/Personagens/?name=ri");

        // Then
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        var body = JsonSerializer.Deserialize<Characters>(responseBody, _jsonOptions);
        Assert.NotNull(body);
        Assert.Equal(expected, body);
    }

    [Fact]
    public async Task GetCharactersWithNoConnectRepository()
    {
        // Given
        ErrorMessageDTO expected = new ErrorMessageDTO
        {
            Title = "Internal Error",
            Message = "A internal error in the server."
        };
        mockCharacter.Setup(c => c.GetCharacters(It.IsAny<CharacterFilterDTO>()))
            .Throws<TaskCanceledException>();
        var client = _builder!.CreateClient();

        // When
        var response = await client.GetAsync("/Personagens/");

        // Then
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        var body = JsonSerializer.Deserialize<ErrorMessageDTO>(responseBody, _jsonOptions);
        Assert.NotNull(body);
        Assert.Equal(expected, body);
    }



    [Fact]
    public async Task GetCharactersWithApiExceptionNotFound()
    {
        // Given
        mockCharacter.Setup(c => c.GetCharacters(It.IsAny<CharacterFilterDTO>()))
            .ThrowsAsync(new CustomApiException404());
        var client = _builder!.CreateClient();

        // When
        var response = await client.GetAsync("/Personagens/");

        // Then
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetCharactersWithApiExceptionBadRequest()
    {
        // Given
        mockCharacter.Setup(c => c.GetCharacters(It.IsAny<CharacterFilterDTO>()))
            .ThrowsAsync(new CustomApiException400());
        var client = _builder!.CreateClient();

        // When
        var response = await client.GetAsync("/Personagens/");

        // Then
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetCharactersWithApiExceptionInternalServerError()
    {
        // Given
        mockCharacter.Setup(c => c.GetCharacters(It.IsAny<CharacterFilterDTO>()))
            .ThrowsAsync(new CustomApiException500());
        var client = _builder!.CreateClient();

        // When
        var response = await client.GetAsync("/Personagens/");

        // Then
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task GetCharactersWithBadQuery()
    {
        // Given
        mockCharacter.Setup(c => c.GetCharacters(It.IsAny<CharacterFilterDTO>()))
            .ReturnsAsync(_repo.GetCharactersWithFilter(new CharacterFilterDTO { }));
        var client = _builder!.CreateClient();

        // When
        var response = await client.GetAsync("/Personagens/?Status=aliv");

        // Then
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseBody);
    }
}

public class CustomApiException404 : ApiException
{
    public CustomApiException404() : base(
        new HttpRequestMessage { }, HttpMethod.Get, null, HttpStatusCode.NotFound, null, null, null) { }
}

public class CustomApiException400 : ApiException
{
   public CustomApiException400() : base(
        new HttpRequestMessage { }, HttpMethod.Get, null, HttpStatusCode.BadRequest, null, null, null)
    { }
}

public class CustomApiException500 : ApiException
{
    public CustomApiException500() : base(
         new HttpRequestMessage { }, HttpMethod.Get, null, HttpStatusCode.InternalServerError, null, null, null)
    { }
}