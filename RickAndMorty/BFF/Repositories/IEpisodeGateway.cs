using BFF.Models;
using BFF.DTOs;
using Refit;

namespace BFF.Repositories;

public interface IEpisodeGateway {
    [Get("/{id}")]
    Task<Episode> GetEpisode(int id);

    [Get("/")]
    Task<Episodes> GetEpisodesByName(EpisodeFilterDTO episodeFilterDTO);
}