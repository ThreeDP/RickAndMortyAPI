namespace BFF.DTOs;
public class ErrorMessageDTO
{
    public string? Title { get; set; }
    public string? Message { get; set; }

    public static bool operator ==(ErrorMessageDTO? left, ErrorMessageDTO? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Title == right.Title
            && left.Message == right.Message;
    }

    public static bool operator !=(ErrorMessageDTO? left, ErrorMessageDTO? right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not ErrorMessageDTO) return false;
        return this == (ErrorMessageDTO)obj;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Title, Message);
    }
}

