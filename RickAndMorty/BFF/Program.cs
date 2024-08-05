using Refit;
using BFF.Repositories;
using BFF.Models;
using BFF.DTOs;
using FluentValidation;
using BFF.Validations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRefitClient<ICharacter>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://rickandmortyapi.com/api/character"));
builder.Services.AddRefitClient<IEpisode>()
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
    IEpisode apiEP,
    ICharacter apiC,
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
    var characters = await apiC.GetCharactersByIds2(newC); 
    return Results.Ok(new ResponseCharactersOfEpisodeDTO {
        EpisodeName = ep.Name,
        EpisodeCode = ep.Ep,
        AirDate = ep.AirDate,
        Characters = characters
    });
})
.WithName("Pegar Personagens por Ep")
.WithOpenApi()
.Produces<ResponseCharactersOfEpisodeDTO>();

app.MapGet("/Personagens/", async (
    ICharacter c,
    IValidator<CharacterFilterDTO> validator,
    [AsParameters] CharacterFilterDTO filter) =>
{
    var valid = validator.Validate(filter);
    if (valid.IsValid is false) {
        string allMessages = valid.ToString();
        return Results.BadRequest(allMessages);
    }
    var res = await c.GetCharacters(filter);

    return Results.Ok(res);
})
.WithName("GetCharacter")
.WithOpenApi()
.Produces<Characters>();

app.Run();
