using System.Runtime.CompilerServices;

namespace BFF.Models;
public class Character
{
    public int          Id { get; set; }
    public string?       Name { get; set; }
    public string?       Status { get; set; }
    public string?       Species { get; set; }
    public string?       Type { get; set; }
    public string?       Gender { get; set; }
    public Location?     Origin {  get; set; }
    public Location?     Location { get; set; }
    public string?       Image { get; set; }
    public List<string> Episode { get; set; } = new List<string>();
    public string?       Url { get; set; }
    public DateTime     Created { get; set; }

    public static bool operator ==(Character? a, Character? b)
    {
        if (a is null && b is null) { return true; }
        if (a is null || b is null) { return false; }
        return (a.Id == b.Id &&
            a.Name == b.Name &&
            a.Status == b.Status &&
            a.Species == b.Species &&
            a.Type == b.Type &&
            a.Gender == b.Gender &&
            a.Origin == b.Origin &&
            a.Location == b.Location &&
            a.Image == b.Image &&
            a.Episode.SequenceEqual(b.Episode) &&
            a.Url == b.Url &&
            a.Created == b.Created);
    }

    public static bool operator !=(Character? a, Character? b)
    {
        return !(a == b);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || !(obj is Character)) {
            return false;
        }
        return this == (Character?)obj;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id,
            Name,
            Status,
            Url,
            Created);
    }
}
