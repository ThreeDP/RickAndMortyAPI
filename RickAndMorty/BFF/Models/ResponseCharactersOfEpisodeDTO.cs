namespace BFF.Models;
public class ResponseCharactersOfEpisodeDTO {
    public string? EpisodeName { get; set; }
    public string? EpisodeCode { get; set; }
    public string? AirDate { get; set; }
    public IEnumerable<Character>? Characters { get; set; }
}

