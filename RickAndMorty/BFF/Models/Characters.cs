namespace BFF.Models;

public class Characters
{
    public Info? Info { get; set; }
    public IEnumerable<Character>? Results { get; set; }

    public static bool operator ==(Characters? left, Characters? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Info == right.Info
            && (left.Results is not null
            && right.Results is not null)
            && left.Results.SequenceEqual(right.Results);
    }

    public static bool operator !=(Characters left, Characters right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not Characters) return false;
        return this == (Characters?)obj;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Info,
            Results);
    }
}
