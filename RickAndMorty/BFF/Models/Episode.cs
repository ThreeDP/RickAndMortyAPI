using System.Text.Json.Serialization;

namespace BFF.Models;
public class Episode {
    public int Id { get; set; }
    public string? Name { get; set; }
    [JsonPropertyName("air_date")]
    public string? AirDate { get; set; }
    [JsonPropertyName("episode")]
    public string? Ep { get; set; }
    public List<string> Characters { get; set; } = new List<string>();
    public string? Url { get; set; }
    public string? Created { get; set; }
}
