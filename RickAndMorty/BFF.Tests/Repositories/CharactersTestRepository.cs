using BFF.DTOs;
using BFF.Models;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

namespace BFF.Tests.Repositories;
internal class CharactersTestRepository
{
    public Characters Characters { get; set; }
    public ResponseCharactersOfEpisodeDTO CharactersByEpisode { get; set; }
    public CharactersTestRepository()
    {
        Characters = new Characters
        {
            Info = new Info
            {
                Count = 3,
                Pages = 1,
                Next = null,
                Prev = null,
            },
            Results = new List<Character> {
                new Character {
                    Id = 1,
                    Name = "Rick Sanchez",
                    Status = "Alive",
                    Species = "Human",
                    Type = "",
                    Gender = "Male",
                    Origin = new Location {
                        Name = "Earth (C-137)",
                        Url = "https://rickandmortyapi.com/api/location/1"
                    },
                    Location = new Location {
                        Name = "Citadel of Ricks",
                        Url = "https://rickandmortyapi.com/api/location/3"
                    },
                    Image = "https://rickandmortyapi.com/api/character/avatar/1.jpeg",
                    Episode = [
                        "https://rickandmortyapi.com/api/episode/1",
                        "https://rickandmortyapi.com/api/episode/2",
                    ],
                    Url = "https://rickandmortyapi.com/api/character/1",
                    Created = DateTime.Parse("2017-11-04T18:48:46.250Z")
                },
                new Character
                {

                    Id = 7,
                    Name = "Abradolf Lincler",
                    Status = "unknown",
                    Species = "Human",
                    Type = "Genetic experiment",
                    Gender = "Male",
                    Origin = new Location {
                        Name = "Earth (Replacement Dimension)",
                        Url = "https://rickandmortyapi.com/api/location/20"
                    },
                    Location = new Location {
                        Name = "Testicle Monster Dimension",
                        Url = "https://rickandmortyapi.com/api/location/21"
                    },
                    Image = "https://rickandmortyapi.com/api/character/avatar/7.jpeg",
                    Episode = [
                        "https://rickandmortyapi.com/api/episode/10",
                        "https://rickandmortyapi.com/api/episode/11"
                    ],
                    Url = "https://rickandmortyapi.com/api/character/7",
                    Created = DateTime.Parse("2017-11-04T19:59:20.523Z")
                },
                new Character
                {
                    Id = 9,
                    Name = "Agency Director",
                    Status = "Dead",
                    Species = "Human",
                    Type = "",
                    Gender = "Male",
                    Origin = new Location {
                        Name = "Earth (Replacement Dimension)",
                        Url = "https://rickandmortyapi.com/api/location/20"
                    },
                    Location = new Location {
                        Name = "Earth (Replacement Dimension)",
                        Url = "https://rickandmortyapi.com/api/location/20"
                    },
                    Image = "https://rickandmortyapi.com/api/character/avatar/9.jpeg",
                    Episode = [
                        "https://rickandmortyapi.com/api/episode/24"
                    ],
                    Url = "https://rickandmortyapi.com/api/character/9",
                    Created = DateTime.Parse("2017-11-04T20:06:54.976Z")
                }
            }
        };

        CharactersByEpisode = new ResponseCharactersOfEpisodeDTO
        {
            EpisodeName = "Pilot",
            AirDate = "December 2, 2013",
            EpisodeCode = "S01E01",
            Characters = Characters.Results
        };
    }

    public async Task<Characters?> GetCharactersWithWaitAsync()
    {
        await Task.Delay(8000);
        return new Characters { };
    }

    public Characters? GetCharactersWithFilter(CharacterFilterDTO filter)
    {
        Characters? c = Characters;
        if (filter.Name is not null)
        {
            c.Results = Characters.Results.Where(n => n.Name.ToLower().StartsWith(filter.Name.ToLower()));
        }
        return c;
    }
}
