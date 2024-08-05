using FluentValidation;
using BFF.DTOs;

namespace BFF.Validations;

public class CharacterFilterValidation : AbstractValidator<CharacterFilterDTO>
{

    private static readonly HashSet<string>
        Status = new HashSet<string> {
            "alive",
            "dead",
            "unknown"
    };

    private static readonly HashSet<string>
        Genders = new HashSet<string> {
            "female",
            "male",
            "genderless",
            "unknown"
    };

    public CharacterFilterValidation()
    {
        RuleFor(c => c.Status).Must(NotValidStatus).WithMessage($"Insert a correct value for Status: [{string.Join(", ", Status)}]");
        RuleFor(c => c.Gender).Must(NotValidGender).WithMessage($"Insert a correct value for Gender: [{string.Join(", ", Genders)}]");
    }

    private bool NotValidGender(string? gender)
    {
        if (gender == null) {
            return true;
        }
        if (Genders.Contains(gender.ToLower())) {
            return true;
        }
        return false;
    }

    private bool NotValidStatus(string? status)
    {
        if (status == null) {
            return true;
        }
        if (Status.Contains(status.ToLower())) {
            return true;
        }
        return false;
    }
}
