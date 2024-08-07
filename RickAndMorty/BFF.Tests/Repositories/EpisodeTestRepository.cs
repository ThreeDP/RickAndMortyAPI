using BFF.Models;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace BFF.Tests.Repositories;
internal class EpisodeTestRepository
{
    public Episodes Episodes { get; set; }
    public EpisodeTestRepository ()
    {
        Episodes = new Episodes
        {
            Info = new Info {
                Count = 2,
                Pages = 1,
                Next = null,
                Prev = null
            },
            Results = new List<Episode> {
                new Episode
                {
                    Id = 1,
                    Name = "Pilot",
                    AirDate = "December 2, 2013",
                    Ep = "S01E01",
                    Characters = [
                        "https://rickandmortyapi.com/api/character/1",
                        "https://rickandmortyapi.com/api/character/2",
                    ],
                    Url = "https://rickandmortyapi.com/api/episode/1",
                    Created = "2017-11-10T12:56:33.798Z"
                },
                new Episode
                {
                    Id = 3,
                    Name = "Anatomy Park",
                    AirDate = "December 16, 2013",
                    Ep = "S01E03",
                    Characters = [
                        "https://rickandmortyapi.com/api/character/1",
                        "https://rickandmortyapi.com/api/character/2",
                    ],
                    Url = "https://rickandmortyapi.com/api/episode/3",
                    Created = "2017-11-10T12:56:34.022Z"
                }
            }
        };

    }
    public ResponseCharactersOfEpisodeDTO GetCharactersByEpisode(string? epname, string? epcode) {
        Episode? ep = null;
        if (epname is not null)
            ep = Episodes.Results.Where(ep => ep.Name.StartsWith(epname)).FirstOrDefault();
        else if (epcode is not null)
            ep = Episodes.Results.Where(ep => ep.Ep.StartsWith(epcode)).FirstOrDefault();
        return new ResponseCharactersOfEpisodeDTO
        {
            EpisodeName = ep.Name,
            EpisodeCode = ep.Ep,
            AirDate = ep.AirDate,
        };
    } 
}
