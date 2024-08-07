using Refit;
using BFF.Repositories;
using BFF.Models;
using BFF.DTOs;
using FluentValidation;
using BFF.Validations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRefitClient<ICharacterGateway>()
    .ConfigureHttpClient(c => {
        c.BaseAddress = new Uri("https://rickandmortyapi.com/api/character");
        c.Timeout = TimeSpan.FromSeconds(5);
    });
builder.Services.AddRefitClient<IEpisodeGateway>()
    .ConfigureHttpClient(e => e.BaseAddress = new Uri("https://rickandmortyapi.com/api/episode"));
builder.Services.AddScoped<IValidator<CharacterFilterDTO>, CharacterFilterValidation>();
builder.Services.AddScoped<IValidator<EpisodeFilterDTO>, EpisodeFilterValidation>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/CharactersByEpisodeName/", async (
    IEpisodeGateway apiEP,
    ICharacterGateway apiC,
    IValidator<EpisodeFilterDTO> validator,
    [AsParameters] EpisodeFilterDTO filter
    ) =>
{
    var obj = validator.Validate(filter);
    if (obj.IsValid is false) {
        return Results.BadRequest(obj.ToString());
    }
    var eps = await apiEP.GetEpisodesByName(filter);
    var ep = eps.Results.FirstOrDefault();
    if (ep is null) { return Results.BadRequest(); }
    var characterList = ep.Characters
    .Select(c =>
    {
        var slice = c.Substring(c.LastIndexOf('/') + 1);
        return slice;
    })
    .Where(slice => slice is not null)
    .ToArray();
    var newC = string.Join(",", characterList);
    var characters = await apiC.GetCharactersByIds("1,2"); 
    return Results.Ok(new ResponseCharactersOfEpisodeDTO {
        EpisodeName = ep.Name,
        EpisodeCode = ep.Ep,
        AirDate = ep.AirDate,
        Characters = characters
    });
})
.WithName("GetCharactersByEpisode")
.WithOpenApi()
.Produces<ResponseCharactersOfEpisodeDTO>();

app.MapGet("/", () => "Hello World!");


app.MapGet("/Personagens/", async (
    ICharacterGateway c,
    IValidator<CharacterFilterDTO> validator,
    [AsParameters] CharacterFilterDTO filter) =>
{
    var valid = validator.Validate(filter);
    if (valid.IsValid is false) {
        return Results.BadRequest(valid.ToString());
    }
    Object? res = null;
    try {
        res = await c.GetCharacters(filter);
    }
    catch (TaskCanceledException)
    {
        return Results.Json(new ErrorMessageDTO
        {
            Title = "Internal Error",
            Message = "A internal error in the server."
        }, statusCode: 500);
    }
    catch (ApiException e)
    {
        return Results.StatusCode((int)e.StatusCode);
    }
    return Results.Ok((Characters?)res);
})
.WithName("GetCharacter")
.WithOpenApi()
.Produces<Characters>();


app.Run();
public partial class Program { }
