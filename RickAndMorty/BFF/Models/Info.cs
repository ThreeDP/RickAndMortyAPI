namespace BFF.Models;

public class Info
{
    public int Count { get; set; }
    public int Pages { get; set; }
    public string? Next { get; set; }
    public string? Prev { get; set; }

    public static bool operator ==(Info? left, Info? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Count == right.Count &&
            left.Pages == right.Pages &&
            left.Next == right.Next &&
            left.Prev == right.Prev;
    }

    public static bool operator !=(Info? left, Info? right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not Info) return false;
        return this == (Info?)obj;
    }

    public override int GetHashCode()
    {
        return 
            HashCode.Combine(Count, Pages, Next, Prev);
    }
}