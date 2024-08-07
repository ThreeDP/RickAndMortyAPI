namespace BFF.Models;

public class Location
{
    public string? Name { get; set; }
    public string? Url { get; set; }

    public static bool operator ==(Location? left, Location? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Name == right.Name &&
            left.Url == right.Url;
    }

    public static bool operator !=(Location? left, Location? right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || !(obj is Location)) return false;
        return this == (Location?)obj;
    }

    public override int GetHashCode() {
        return HashCode.Combine(Name, Url);
    }
}