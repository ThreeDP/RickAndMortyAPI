using Refit;

namespace BFF.DTOs;
public class EpisodeFilterDTO : PageFilterDTO {
    [AliasAs("name")]
    public string? Name { get; set; }
    [AliasAs("episode")]
    public string? Episode { get; set; }
}

