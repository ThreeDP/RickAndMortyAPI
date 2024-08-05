using BFF.DTOs;
using FluentValidation;

namespace BFF.Validations;
public class EpisodeFilterValidation : AbstractValidator<EpisodeFilterDTO> {
    public EpisodeFilterValidation() {
        RuleFor(ep => ep.Episode).Null().When(ep => ep.Name is not null);
        RuleFor(ep => ep.Name).Null().When(ep => ep.Episode is not null);
        // RuleFor(ep => ep.Name).Null().When(ep => ep.Episode is null);
    }

}
